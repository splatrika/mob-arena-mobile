using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public abstract class Mob<TConfiguration> : IDamageable,
        IUpdatable, IWalking, IReusable<TConfiguration>, IDisposable
        where TConfiguration : MobConfiguration
    {
        public int RewardPoints { get; private set; }
        public Vector3 Position => FollowingStrategy.Position;
        public Vector3 Direction { get; private set; }
        public bool IsWalking { get; private set; }
        public bool IsDied => _damageable.IsDied;
        public int Health => _damageable.Health;
        public bool Active { get; private set; }

        protected IFollowingStrategy FollowingStrategy { get; private set; }
        protected IAttackingStrategy AttackingStrategy { get; private set; }

        private readonly DamageablePartial _damageable;
        private readonly ILogger _logger;

        public event Action<int> HealthUpdated;
        public event Action Damaged;
        public event Action Died;
        public event Action MovementStarted;
        public event Action MovementStopped;
        public event Action<Vector3> DirectionUpdated;
        public event Action Activated;
        public event Action Deactivated;

        protected abstract void OnStart(TConfiguration configuration,
            IFollowingStrategy following, IAttackingStrategy attacking);
        protected virtual void OnDispose() { }


        public Mob(
            IFollowingStrategy followingStrategy,
            IAttackingStrategy attackingStrategy,
            ILogger logger)
        {
            FollowingStrategy = followingStrategy;
            AttackingStrategy = attackingStrategy;
            _logger = logger;
            _damageable = new DamageablePartial();

            FollowingStrategy.Arrived += OnArrived;
            FollowingStrategy.StartedFollowing += OnStartedFollowing;
            FollowingStrategy.DirectionUpdated += OnDirectionUpdated;
            FollowingStrategy.Frozen += OnFrozen;
            FollowingStrategy.Resumed += OnResumed;
            _damageable.Damaged += OnDamaged;
            _damageable.HealthUpdated += OnHealthUpdate;
            _damageable.Died += OnDied;

            _logger.Log($"{GetType().Name} was instantiated");
        }


        public void Dispose()
        {
            FollowingStrategy.Arrived -= OnArrived;
            FollowingStrategy.StartedFollowing -= OnStartedFollowing;
            FollowingStrategy.DirectionUpdated -= OnDirectionUpdated;
            FollowingStrategy.Frozen -= OnFrozen;
            FollowingStrategy.Resumed -= OnResumed;
            _damageable.Damaged -= OnDamaged;
            _damageable.HealthUpdated -= OnHealthUpdate;
            _damageable.Died -= OnDied;
            if (FollowingStrategy is IDisposable following)
            {
                following.Dispose();
            }
            if (AttackingStrategy is IDisposable attacking)
            {
                attacking.Dispose();
            }
            OnDispose();
            _logger.Log($"{GetType().Name} was disposed");
        }


        public void Start(TConfiguration configuration)
        {
            if (Active)
            {
                _logger.LogError(GetType().Name, "Already active");
            }
            FollowingStrategy.SetPosition(configuration.Position);
            FollowingStrategy.SetRegenerationTime(
                configuration.WalkingRegenerationTime);
            _damageable.Setup(new DamageableConfiguration(
                lifes: configuration.Health,
                allowedDamagers: x => x is FriendBullet));
            OnStart(configuration, FollowingStrategy, AttackingStrategy);
            RewardPoints = configuration.RewardPoints;
            Active = true;
            Activated?.Invoke();
            _logger.Log($"{GetType().Name} was started");
        }


        public void Damage(IDamager damager)
        {
            _damageable.Damage(damager);
        }


        public void Update(float deltaTime)
        {
            if (Active)
            {
                FollowingStrategy.Update(deltaTime);
                AttackingStrategy.Update(deltaTime);
            }
        }


        private void OnArrived()
        {
            AttackingStrategy.Start();
            IsWalking = false;
            MovementStopped?.Invoke();
        }


        private void OnStartedFollowing()
        {
            AttackingStrategy.Stop();
            IsWalking = true;
            MovementStarted?.Invoke();
        }


        private void OnDirectionUpdated(Vector3 direction)
        {
            Direction = direction;
            DirectionUpdated?.Invoke(direction);
        }


        private void OnDamaged()
        {
            FollowingStrategy.Hit();
            Damaged?.Invoke();
        }


        private void OnHealthUpdate(int health)
        {
            HealthUpdated?.Invoke(health);
        }


        private void OnFrozen()
        {
            MovementStopped?.Invoke();
        }


        private void OnResumed()
        {
            MovementStarted?.Invoke();
        }


        private void OnDied()
        {
            FollowingStrategy.Stop();
            AttackingStrategy.Stop();
            Died?.Invoke();
            Active = false;
            Deactivated?.Invoke();
        }
    }
}
