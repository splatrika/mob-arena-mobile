using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class PlayerCharacter : IPlayerCharacter, IUpdatable
    {
        public bool IsDied => _damagable.IsDied;
        public int Health => _damagable.Health;
        public Vector3 Position { get; private set; }
        public Vector3 Direction { get; private set; }

        private readonly RegeneratableAction _shooting;
        private readonly IFriendBulletService _friendBulletService;
        private readonly ITimeScaleService _timeScaleService;
        private readonly IDamagablePartial _damagable;
        private readonly ILogger _logger;

        public event Action Died;
        public event Action<int> HealthUpdated;
        public event Action Shot;
        public event Action<Vector3> Rotated;


        public PlayerCharacter(
            PlayerCharacterConfiguration configuration,
            IFriendBulletService friendBulletService,
            ITimeScaleService timeScaleService,
            ILogger logger,
            IDamagablePartial damagable)
        {
            _friendBulletService = friendBulletService;
            _timeScaleService = timeScaleService;
            _logger = logger;
            _damagable = damagable;

            Position = configuration.Position;
            Direction = configuration.Direction;
            _shooting = new RegeneratableAction(
                regenerationTime: configuration.ShootRegenerationTime,
                action: Shoot,
                timeScaleService: _timeScaleService);

            var damagableConfiguration = new DamagableConfiguration(
                lifes: configuration.Health,
                allowedDamagers: damager => !(damager is IFriendBullet));
            _damagable.Setup(damagableConfiguration);

            _damagable.HealthUpdated += x => HealthUpdated?.Invoke(x);
            _damagable.Died += () => Died?.Invoke();
        }


        public void SetDirection(Vector2 direction)
        {
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
            _damagable.Damage(damager);
        }


        public void Update(float deltaTime)
        {
            _shooting.Update(deltaTime);
        }


        private void Shoot()
        {
            _friendBulletService.Spawn(Position, Direction);
        }
    }
}
