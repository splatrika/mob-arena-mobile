using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class WalkingMob: IDamager, IUpdatable, IHealth, IDamagable
    {
        public Vector3 Position => _following.Position;
        public bool IsAtacking { get; private set; }
        public int DamageAmount { get; }
        public bool IsDied =>_damagable.IsDied;
        public int Health => _damagable.Health;
        public bool Active { get; private set; }

        private readonly IPlayerCharacter _playerCharacter;
        private readonly IFollowingPartial _following;
        private readonly IDamagablePartial _damagable;
        private readonly RegeneratableAction _atacking;

        public event Action Started;
        public event Action<Vector3> MovementStarted;
        public event Action MovementStopped;
        public event Action Atacked;
        public event Action Died;
        public event Action<int> HealthUpdated;


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

            _following.Arrived += OnArrived;
            _following.MovementStarted += x => MovementStarted?.Invoke(x);
            _following.MovementStopped += () => MovementStopped?.Invoke();
            _damagable.HealthUpdated += x => HealthUpdated?.Invoke(x);
            _damagable.Died += OnDied;
        }


        public void Start(WalkingMobConfiguration configuration)
        {
            if (Active)
            {
                throw new InvalidOperationException("Already active");
            }

            _atacking.Reset(configuration.AtackRegenerationTime);

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

            Active = true;

            Started?.Invoke();
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


        private void OnDied()
        {
            Active = false;
            Died?.Invoke();
        }


        private void Atack()
        {
            _playerCharacter.Damage(this);
            Atacked?.Invoke();
        }
    }
}
