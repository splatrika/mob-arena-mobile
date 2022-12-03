using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class ShootingStrategy : IAttackingStrategy
    {
        private int _bulletsDamage;
        private float _bulletsSpeed;
        private IPositionProvider _shootPoint;
        private IPositionProvider _target;
        private readonly IBulletService _bulletService;
        private readonly RegeneratingAction _shooting;

        public event Action Shot;
        public event Action<Vector3> DirectionUpdated;


        public ShootingStrategy(
            IBulletService bulletService,
            ITimeScaleService timeScaleService)
        {
            _bulletService = bulletService;
            _shooting = new RegeneratingAction(
                action: Shoot,
                timeScaleService: timeScaleService);
        }


        public void Setup(
            int bulletsDamage,
            float bulletsSpeed,
            IPositionProvider shootPoint,
            IPositionProvider target,
            float regenerationTime)
        {
            _bulletsDamage = bulletsDamage;
            _bulletsSpeed = bulletsSpeed;
            _shootPoint = shootPoint;
            _target = target;

            _shooting.Reset(regenerationTime);
        }


        public void Start()
        {
            _shooting.Start();
        }


        public void Stop()
        {
            _shooting.Stop();
        }


        public void Update(float deltaTime)
        {
            _shooting.Update(deltaTime);
        }


        private void Shoot()
        {
            var direction = _target.Position - _shootPoint.Position;
            DirectionUpdated?.Invoke(direction);

            _bulletService.Spawn(new BulletConfiguration(
                position: _shootPoint.Position,
                direction: direction,
                speed: _bulletsSpeed,
                damage: _bulletsDamage));
        }
    }
}
