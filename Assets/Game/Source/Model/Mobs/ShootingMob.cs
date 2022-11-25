using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class ShootingMob : IReusable<ShootingMobConfiguration>,
        IUpdatable, IDamagable, IHealth, IEnemy
    {
        public Vector3 Position => _following.Position;
        public bool Active { get; private set; }
        public bool IsDied => _damagable.IsDied;
        public int Health => _damagable.Health;
        public float BulletsSpeed { get; private set; }
        public int BulletsDamage { get; private set; }
        public IPositionProvider ShotPoint { get; private set; }
        public bool IsMoving => _following.IsMoving;
        public int RewardPoints { get; private set; }

        private readonly IDamagablePartial _damagable;
        private readonly IFollowingPartial _following;
        private readonly IBulletService _bulletService;
        private readonly ITimeScaleService _timeScaleService;
        private readonly IPlayerCharacter _playerCharacter;
        private readonly RegeneratableAction _shooting;
        private Action<IEnemy> _enemyDied;

        public event Action Started;
        public event Action Died;
        public event Action<int> HealthUpdated;
        public event Action Damaged;
        public event Action MovementStarted;
        public event Action MovementStopped;
        public event Action<Vector3> DirectionUpdated;
        public event Action ShootingStarted;
        public event Action Shot;
        public event Action Activated;
        public event Action Deactivated;
        event Action<IEnemy> IEnemy.Died
        {
            add => _enemyDied += value;
            remove => _enemyDied -= value;
        }


        public ShootingMob(
            IDamagablePartial damagable,
            IFollowingPartial following,
            IBulletService bulletService,
            ITimeScaleService timeScaleService,
            IPlayerCharacter playerCharacter)
        {
            _damagable = damagable;
            _following = following;
            _bulletService = bulletService;
            _timeScaleService = timeScaleService;
            _playerCharacter = playerCharacter;

            _shooting = new RegeneratableAction(
                regenerationTime: 0,
                action: Shoot,
                timeScaleService: _timeScaleService);

            _damagable.Died += OnDied;
            _damagable.HealthUpdated += x => HealthUpdated?.Invoke(x);
            _damagable.Damaged += OnDamaged;

            _following.MovementStarted += () => MovementStarted?.Invoke();
            _following.MovementStopped += () => MovementStopped?.Invoke();
            _following.DirectionUpdated += x => DirectionUpdated?.Invoke(x);
            _following.Arrived += OnArrived;
        }


        public void Start(ShootingMobConfiguration configuration)
        {
            if (Active)
            {
                throw new InvalidOperationException("Already active");
            }

            _shooting.Reset(configuration.GunRegenerationTime);

            var damagableConfiguration = new DamagableConfiguration(
                lifes: configuration.Health,
                allowedDamagers: damager => damager is IFriendBullet);
            _damagable.Setup(damagableConfiguration);

            var followingConfiguration = new FollowingConfiguration(
                startPoint: configuration.Start,
                followTarget: configuration.ShootingPosition,
                minDistance: 0,
                speed: configuration.MovementSpeed,
                regenerationTime: configuration.MovementRegenerationTime);
            _following.Start(followingConfiguration);

            BulletsSpeed = configuration.BulletsSpeed;
            BulletsDamage = configuration.BulletsDamage;
            ShotPoint = configuration.ShotPoint;
            RewardPoints = configuration.RewardPoints;

            Active = true;
            Started?.Invoke();
            Activated?.Invoke();
        }


        public void Damage(IDamager damager)
        {
            _damagable.Damage(damager);
        }


        public void Update(float deltaTime)
        {
            if (!Active)
            {
                return;
            }
            _shooting.Update(deltaTime);
            _following.Update(deltaTime);
        }


        private void OnDamaged()
        {
            _following.Hit();
            Damaged?.Invoke();
        }


        private void OnDied()
        {
            Active = false;
            Died?.Invoke();
            _enemyDied?.Invoke(this);
            Deactivated?.Invoke();
        }


        private void OnArrived()
        {
            _shooting.Start();
            var directionToPlayer = _playerCharacter.Center - Position;
            directionToPlayer.y = 0;
            DirectionUpdated?.Invoke(directionToPlayer.normalized);
            ShootingStarted?.Invoke();
        }


        private void Shoot()
        {
            var shotPoint = ShotPoint.Position;
            var playerCenter = _playerCharacter.Position
                + _playerCharacter.CenterOffset;
            var directionToPlayer = playerCenter - shotPoint;

            var configuration = new BulletConfiguration(
                position: shotPoint,
                direction: directionToPlayer,
                speed: BulletsSpeed,
                damage: BulletsDamage);

            _bulletService.Spawn(configuration);

            Shot?.Invoke();
        }
    }
}
