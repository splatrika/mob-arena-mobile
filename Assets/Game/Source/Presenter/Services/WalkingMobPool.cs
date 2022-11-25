using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class WalkingMobPool : PoolService<WalkingMobConfiguration,
        WalkingMob>
    {
        private readonly IPlayerCharacter _playerCharacter;
        private readonly ITimeScaleService _timeScaleService;
        private readonly ILogger _logger;
        private readonly INavigationService _navigationService;
        private readonly NavigationSettings _navigationSettings;


        public WalkingMobPool(
            int poolSize,
            IPlayerCharacter playerCharacter,
            ITimeScaleService timeScaleService,
            ILogger logger,
            INavigationService navigationService,
            NavigationSettings navigationSettings)
            : base(poolSize, logger)
        {
            _playerCharacter = playerCharacter;
            _timeScaleService = timeScaleService;
            _logger = logger;
            _navigationService = navigationService;
            _navigationSettings = navigationSettings;
            CreateObjects();
        }


        protected override WalkingMob CreateInstance()
        {
            var navigationConfiguration = new NavigationConfiguration(
                _navigationSettings.PathBufferSize);
            var navigationPartial = new NavigationPartial(
                navigationConfiguration, _navigationService, _logger,
                _timeScaleService);

            return new WalkingMob(_playerCharacter,
                new FollowingPartial(navigationPartial, _timeScaleService),
                new DamageablePartial(),
                _timeScaleService);
        }
    }
}
