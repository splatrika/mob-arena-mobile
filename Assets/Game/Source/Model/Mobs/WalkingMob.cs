using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class WalkingMob: IReusable<WalkingMobConfiguration>,
        IDamager, IUpdatable, IHealth, IDamagable, IEnemy
    {
        public Vector3 Position => _following.Position;
        public bool IsAtacking { get; private set; }
        public int DamageAmount { get; private set; }
        public bool IsDied =>_damagable.IsDied;
        public int Health => _damagable.Health;
        public bool Active { get; private set; }
        public bool IsMoving => _following.IsMoving;
        public int RewardPoints { get; private set; }

        private readonly IPlayerCharacter _playerCharacter;
        private readonly IFollowingPartial _following;
        private readonly IDamagablePartial _damagable;
        private readonly RegeneratableAction _atacking;
        private Action<IEnemy> _enemyDied;

        public event Action Started;
        public event Action MovementStarted;
        public event Action MovementStopped;
        public event Action<Vector3> DirectionUpdated;
        public event Action Atacked;
        public event Action Died;
        public event Action<int> HealthUpdated;
        public event Action Damaged;
        public event Action Activated;
        public event Action Deactivated;
        event Action<IEnemy> IEnemy.Died
        {
            add => _enemyDied += value;
            remove => _enemyDied -= value;
        }


        public WalkingMob(
            IPlayerCharacter playerCharacter,
            IFollowingPartial followingPartial,
            IDamagablePartial damagable,
            ITimeScaleService timeScaleService)
        {
            _playerCharacter = playerCharacter;
            _following = followingPartial;
            _damagable = damagable;

            _atacking = new RegeneratableAction(
                regenerationTime: 0,
                action: Atack,
                timeScaleService: timeScaleService);

            _damagable.HealthUpdated += x => HealthUpdated?.Invoke(x);
            _damagable.Died += OnDied;
            _damagable.Damaged += OnDamaged;

            _following.Arrived += OnArrived;
            _following.MovementStarted += () => MovementStarted?.Invoke();
            _following.MovementStopped += () => MovementStopped?.Invoke();
            _following.DirectionUpdated += x => DirectionUpdated?.Invoke(x);
        }


        public void Start(WalkingMobConfiguration configuration)
        {
            if (Active)
            {
                throw new InvalidOperationException("Already active");
            }

            _atacking.Reset(configuration.AtackRegenerationTime);
            DamageAmount = configuration.AtackDamage;

            var followingConfiguration = new FollowingConfiguration(
                startPoint: configuration.StartPoint,
                followTarget: _playerCharacter.Position,
                minDistance: configuration.AtackDistance,
                speed: configuration.Speed,
                regenerationTime: configuration.WalkingRegenerationTime);
            _following.Start(followingConfiguration);

            var damagableConfiguration = new DamagableConfiguration(
                lifes: configuration.Health,
                allowedDamagers: damager => damager is IFriendBullet);
            _damagable.Setup(damagableConfiguration);

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
            _following.Update(deltaTime);
            _atacking.Update(deltaTime);
        }


        private void OnArrived()
        {
            IsAtacking = true;
            _atacking.Start();
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


        private void Atack()
        {
            _playerCharacter.Damage(this);
            Atacked?.Invoke();
        }
    }
}
