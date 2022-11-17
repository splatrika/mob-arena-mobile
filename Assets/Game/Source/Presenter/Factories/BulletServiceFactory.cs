using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class BulletServiceFactory : IFactory<BulletService>
    {
        private readonly BulletsSettings _settings;
        private readonly IRaycastService _raycastService;
        private readonly ITimeScaleService _timeScaleService;
        private readonly ILogger _logger;


        public BulletServiceFactory(
            BulletsSettings settings,
            IRaycastService raycastService,
            ITimeScaleService timeScaleService,
            ILogger logger)
        {
            _settings = settings;
            _raycastService = raycastService;
            _timeScaleService = timeScaleService;
            _logger = logger;
        }


        public BulletService Create()
        {
            var configuration = new BulletServiceConfiguration(
                poolSize: _settings.PoolSize,
                layerMask: _settings.LayerMask,
                bulletsLifeTime: _settings.LifeTime);

            var service = new BulletService(configuration,
                _raycastService, _logger, _timeScaleService);

            if (_settings.Prefab == null)
            {
                _logger.LogError(nameof(BulletServiceFactory),
                    "Bullet prefab is not setted in BulletSettings");
            }

            foreach (var bullet in service.Objects)
            {
                var presenter = GameObject.Instantiate(_settings.Prefab);
                presenter.Init(bullet);
            }

            return service;
        }
    }
}
