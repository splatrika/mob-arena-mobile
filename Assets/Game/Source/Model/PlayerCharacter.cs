using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class PlayerCharacter : IPlayerCharacter, IUpdatable
    {
        public bool IsDied => _damageable.IsDied;
        public int Health => _damageable.Health;
        public Vector3 Position { get; private set; }
        public Vector3 Direction { get; private set; }
        public Vector3 CenterOffset { get; }
        public Vector3 Center => Position + CenterOffset;

        private readonly RegeneratableAction _shooting;
        private readonly IFriendBulletService _friendBulletService;
        private readonly ITimeScaleService _timeScaleService;
        private readonly IDamageablePartial _damageable;
        private readonly ILogger _logger;

        public event Action Died;
        public event Action<int> HealthUpdated;
        public event Action Damaged;
        public event Action Shot;
        public event Action<Vector3> Rotated;


        public PlayerCharacter(
            PlayerCharacterConfiguration configuration,
            IFriendBulletService friendBulletService,
            ITimeScaleService timeScaleService,
            ILogger logger,
            IDamageablePartial damageable)
        {
            _friendBulletService = friendBulletService;
            _timeScaleService = timeScaleService;
            _logger = logger;
            _damageable = damageable;

            Position = configuration.Position;
            Direction = configuration.Direction;
            CenterOffset = configuration.CenterOffset;
            _shooting = new RegeneratableAction(
                regenerationTime: configuration.ShootRegenerationTime,
                action: Shoot,
                timeScaleService: _timeScaleService);

            var damageableConfiguration = new DamageableConfiguration(
                lifes: configuration.Health,
                allowedDamagers: damager => !(damager is IFriendBullet));
            _damageable.Setup(damageableConfiguration);

            _damageable.HealthUpdated += x => HealthUpdated?.Invoke(x);
            _damageable.Died += OnDied;
            _damageable.Damaged += () => Damaged?.Invoke();
        }


        public void SetDirection(Vector2 direction)
        {
            if (IsDied)
            {
                _logger.LogWarning(nameof(PlayerCharacter),
                    "Died player character can't rotate");
                return;
            }
            Direction = new Vector3(direction.x, 0, direction.y)
                .normalized;
            Rotated?.Invoke(Direction);
        }


        public void StartShooting()
        {
            if (IsDied)
            {
                _logger.LogWarning(nameof(PlayerCharacter),
                    "Died player character can't start shooting");
                return;
            }
            _shooting.Start();
        }


        public void StopShooting()
        {
            _shooting.Stop();
        }


        public void Damage(IDamager damager)
        {
            _damageable.Damage(damager);
        }


        public void Update(float deltaTime)
        {
            _shooting.Update(deltaTime);
        }


        private void Shoot()
        {
            _friendBulletService.Spawn(Center, Direction);
            Shot?.Invoke();
        }


        private void OnDied()
        {
            StopShooting();
            Died?.Invoke();
        }
    }
}
