using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public abstract class Mob<TConfiguration> : IHealth, IDamageable,
        IUpdatable, IReusable<TConfiguration>, IDisposable
        where TConfiguration : MobConfiguration
    {
        public int RewardPoints { get; private set; }
        public Vector3 Position => _followingStrategy.Position;
        public bool IsDied => _damageable.IsDied;
        public int Health => _damageable.Health;
        public bool Active { get; private set; }

        private IFollowingStrategy _followingStrategy;
        private IAttackingStrategy _attackingStrategy;
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


        public Mob(
            IFollowingStrategy followingStrategy,
            IAttackingStrategy attackingStrategy,
            ILogger logger)
        {
            _followingStrategy = followingStrategy;
            _attackingStrategy = attackingStrategy;
            _logger = logger;
            _damageable = new DamageablePartial();

            _followingStrategy.Arrived += OnArrived;
            _followingStrategy.StartedFollowing += OnStartedFollowing;
            _followingStrategy.DirectionUpdated += OnDirectionUpdated;
            _damageable.Damaged += OnDamaged;
            _damageable.HealthUpdated += OnHealthUpdate;
            _damageable.Died += OnDied;
        }


        public void Dispose()
        {
            _followingStrategy.Arrived -= OnArrived;
            _followingStrategy.StartedFollowing -= OnStartedFollowing;
            _followingStrategy.DirectionUpdated -= OnDirectionUpdated;
            _damageable.Damaged -= OnDamaged;
            _damageable.HealthUpdated -= OnHealthUpdate;
            _damageable.Died -= OnDied;
            if (_followingStrategy is IDisposable following)
            {
                following.Dispose();
            }
            if (_attackingStrategy is IDisposable attacking)
            {
                attacking.Dispose();
            }
            _logger.Log($"{GetType().Name} was disposed");
        }


        public void Start(TConfiguration configuration)
        {
            if (Active)
            {
                _logger.LogError(GetType().Name, "Already active");
            }
            _followingStrategy.SetPosition(configuration.Position);
            _damageable.Setup(new DamageableConfiguration(
                lifes: configuration.Health,
                allowedDamagers: x => x is FriendBullet));
            OnStart(configuration, _followingStrategy, _attackingStrategy);
            RewardPoints = configuration.RewardPoints;
            Active = true;
            Activated?.Invoke();
        }


        public void Damage(IDamager damager)
        {
            _damageable.Damage(damager);
        }


        public void Update(float deltaTime)
        {
            _followingStrategy.Update(deltaTime);
            _attackingStrategy.Update(deltaTime);
        }


        private void OnArrived()
        {
            _attackingStrategy.Start();
            MovementStopped?.Invoke();
        }


        private void OnStartedFollowing()
        {
            _attackingStrategy.Stop();
            MovementStarted?.Invoke();
        }


        private void OnDirectionUpdated(Vector3 direction)
        {
            DirectionUpdated?.Invoke(direction);
        }


        private void OnDamaged()
        {
            Damaged?.Invoke();
        }


        private void OnHealthUpdate(int health)
        {
            HealthUpdated?.Invoke(health);
        }


        private void OnDied()
        {
            _followingStrategy.Stop();
            _attackingStrategy.Stop();
            Died?.Invoke();
            Active = false;
            Deactivated?.Invoke();
        }
    }
}
