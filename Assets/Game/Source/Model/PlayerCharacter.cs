using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class PlayerCharacter : IUpdatable, IDamagable, IHealth
    {
        public bool IsDied => Health == 0;
        public int Health { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 Direction { get; private set; }

        private readonly RegeneratableAction _shooting;
        private readonly IFriendBulletService _friendBulletService;
        private readonly ITimeScaleService _timeScaleService;
        private readonly ILogger _logger;

        public event Action Died;
        public event Action<int> HealthUpdated;
        public event Action Shot;
        public event Action<Vector3> Rotated;


        public PlayerCharacter(
            PlayerCharacterConfiguration configuration,
            IFriendBulletService friendBulletService,
            ITimeScaleService timeScaleService, ILogger logger)
        {
            _friendBulletService = friendBulletService;
            _timeScaleService = timeScaleService;
            _logger = logger;

            Health = configuration.Health;
            Position = configuration.Position;
            Direction = configuration.Direction;
            _shooting = new RegeneratableAction(
                regenerationTime: configuration.ShootRegenerationTime,
                action: Shoot,
                timeScaleService: _timeScaleService);
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
            if (damager is IFriendBullet)
            {
                return;
            }
            Health -= damager.DamageAmount;
            Health = Mathf.Max(0, Health);
            HealthUpdated?.Invoke(Health);
            if (Health == 0)
            {
                _shooting.Stop();
                Died?.Invoke();
            }
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
