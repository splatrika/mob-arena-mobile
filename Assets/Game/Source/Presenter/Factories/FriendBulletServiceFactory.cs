using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class FriendBulletServiceFactory : IFactory<FriendBulletService>
    {
        private readonly ILogger _logger;
        private readonly IRaycastService _raycastService;
        private readonly ITimeScaleService _timeScaleService;
        private readonly PlayerSettings _playerSettings;
        private readonly BulletsSettings _bulletsSettings;


        public FriendBulletServiceFactory(
            ILogger logger,
            IRaycastService raycastService,
            ITimeScaleService timeScaleService,
            PlayerSettings playerSettings,
            BulletsSettings bulletsSettings)
        {
            _raycastService = raycastService;
            _timeScaleService = timeScaleService;
            _playerSettings = playerSettings;
            _bulletsSettings = bulletsSettings;
            _logger = logger;
        }


        public FriendBulletService Create()
        {
            var configuration = new FriendBulletServiceConfiguration(
                poolSize: _bulletsSettings.PoolSize,
                layerMask: _playerSettings.BulletsLayerMask,
                bulletsLifeTime: _bulletsSettings.LifeTime,
                speed: _playerSettings.BulletSpeed,
                damage: _playerSettings.BulletDamage);

            var service = new FriendBulletService(configuration,
                _logger, _raycastService, _timeScaleService);

            var prefab = Resources.Load<BulletPresenter>(
                _playerSettings.BulletPrefab);
            if (!prefab)
            {
                _logger.LogError(nameof(FriendBulletServiceFactory),
                    "Unable to load bullet prefab");
                return service;
            }

            foreach (var bullet in service.Objects)
            {
                var presenter = GameObject.Instantiate(prefab);
                presenter.Init(bullet);
            }

            return service;
        }
    }
}
