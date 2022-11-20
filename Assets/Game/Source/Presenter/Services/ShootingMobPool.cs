using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class ShootingMobPool
        : PoolService<ShootingMobConfiguration, ShootingMob>
    {
        private readonly IBulletService _bulletService;
        private readonly ITimeScaleService _timeScaleService;
        private readonly IPlayerCharacter _playerCharacter;
        private readonly INavigationService _navigationService;
        private readonly ILogger _logger;
        private readonly NavigationSettings _navigationSettings;


        public ShootingMobPool(
            int poolSize,
            IBulletService bulletService,
            ITimeScaleService timeScaleService,
            IPlayerCharacter playerCharacter,
            ILogger logger,
            INavigationService navigationService,
            NavigationSettings navigationSettings)
            : base(poolSize, logger)
        {
            _bulletService = bulletService;
            _timeScaleService = timeScaleService;
            _playerCharacter = playerCharacter;
            _logger = logger;
            _navigationService = navigationService;
            _navigationSettings = navigationSettings;
            CreateObjects();
        }


        protected override ShootingMob CreateInstance()
        {
            var navigationConfiguration = new NavigationConfiguration(
                _navigationSettings.PathBufferSize);
            var navigationPartial = new NavigationPartial(
                navigationConfiguration, _navigationService,
                _logger, _timeScaleService);

            return new ShootingMob(
                new DamagablePartial(),
                new FollowingPartial(navigationPartial, _timeScaleService),
                _bulletService,
                _timeScaleService,
                _playerCharacter);
        }
    }
}
