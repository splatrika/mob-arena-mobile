using System;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class WalkingMobFromSceneFactory : IFactory<WalkingMob>
    {
        private readonly IPlayerCharacter _playerCharacter;
        private readonly ITimeScaleService _timeScaleService;
        private readonly INavigationService _navigationService;
        private readonly NavigationSettings _navigationSettings;
        private readonly ILogger _logger;


        public WalkingMobFromSceneFactory(
            IPlayerCharacter playerCharacter,
            ITimeScaleService timeScaleService,
            INavigationService navigationService,
            NavigationSettings navigationSettings,
            ILogger logger)
        {
            _playerCharacter = playerCharacter;
            _timeScaleService = timeScaleService;
            _navigationService = navigationService;
            _navigationSettings = navigationSettings;
            _logger = logger;
        }


        public WalkingMob Create()
        {
            var settingsFromScene = GameObject
                .FindObjectOfType<WalkingMobMonoSettings>();
            if (!settingsFromScene)
            {
                throw new InvalidOperationException("Unable to create walking mob" +
                    "because there is no WalkingMobMonoSetting on the scene");
            }

            var configuration = new WalkingMobConfiguration(
                startPoint: settingsFromScene.transform.position,
                speed: settingsFromScene.Speed,
                attackDistance: settingsFromScene.AttackDistance,
                attackRegenerationTime: settingsFromScene.AttackRegenerationTime,
                attackDamage: settingsFromScene.AttackDamage,
                health: settingsFromScene.Health,
                walkingRegenerationTime:
                    settingsFromScene.WalkingRegenerationTime,
                rewardPoints: settingsFromScene.RewardPoints);

            var navigationConfiguration = new NavigationConfiguration(
                _navigationSettings.PathBufferSize);
            var navigation = new NavigationPartial(navigationConfiguration,
                _navigationService, _logger, _timeScaleService);

            var model = new WalkingMob(_playerCharacter,
                new FollowingPartial(navigation, _timeScaleService),
                new DamageablePartial(),
                _timeScaleService);

            settingsFromScene.Presenter.Init(model);

            model.Start(configuration);

            return model;
        }
    }
}
