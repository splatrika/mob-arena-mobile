using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class BulletService : PoolService<BulletConfiguration, Bullet>,
        IBulletService, IUpdatable, IFixedUpdatable
    {
        private readonly float _bulletsLifeTime;
        private readonly int _layerMask;
        private readonly IRaycastService _raycastService;
        private readonly ITimeScaleService _timeScaleService;

        public event IndexAction Updated;


        public BulletService(
            BulletServiceConfiguration configuration,
            IRaycastService raycastService,
            ILogger logger,
            ITimeScaleService timeScaleService)
            : base(configuration.PoolSize, logger)
        {
            _bulletsLifeTime = configuration.BulletsLifeTime;
            _layerMask = configuration.LayerMask;
            _raycastService = raycastService;
            _timeScaleService = timeScaleService;
            CreateObjects();
        }


        protected sealed override Bullet CreateInstance()
        {
            return new Bullet(_bulletsLifeTime, _layerMask, _raycastService,
                _timeScaleService);
        }


        public void Update(float deltaTime)
        {
            for (var i = 0; i < Objects.Count; i++)
            {
                var bullet = Objects[i];
                if (bullet.Active)
                {
                    bullet.Update(deltaTime);
                    Updated?.Invoke(i);
                }
            }
        }


        public void FixedUpdate(float deltaTime)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                var bullet = Objects[i];
                if (bullet.Active)
                {
                    bullet.FixedUpdate(deltaTime);
                }
            }
        }

    }
}
