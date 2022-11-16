using System;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class ShootingMobFromSceneFactory : IFactory<ShootingMob>
    {
        private readonly IBulletService _bulletService;
        private readonly ITimeScaleService _timeScaleService;
        private readonly IPlayerCharacter _playerCharacter;
        private readonly ILogger _logger;
        private readonly INavigationService _navigationService;
        private readonly NavigationSettings _navigationSettings;

        public ShootingMobFromSceneFactory(
            IBulletService bulletService,
            ITimeScaleService timeScaleService,
            IPlayerCharacter playerCharacter,
            ILogger logger,
            INavigationService navigationService,
            NavigationSettings navigationSettings)
        {
            _bulletService = bulletService;
            _timeScaleService = timeScaleService;
            _playerCharacter = playerCharacter;
            _logger = logger;
            _navigationService = navigationService;
            _navigationSettings = navigationSettings;
        }


        public ShootingMob Create()
        {
            var spawnSettings = GameObject
                .FindObjectOfType<ShootingMobSpawnMonoSettings>();
            if (!spawnSettings)
            {
                throw new NullReferenceException("Unable to create " +
                    "ShootingMob because there is no " +
                    "ShootingMobSpawnMonoSettings on the scene");
            }

            var mobSettings = spawnSettings.Settings;
            var configuration = new ShootingMobConfiguration(
                health: mobSettings.Health,
                bulletsSpeed: mobSettings.BulletsSpeed,
                bulletsDamage: mobSettings.BulletsDamage,
                gunRegenerationTime: mobSettings.GunRegenerationTime,
                start: spawnSettings.transform.position,
                movementSpeed: mobSettings.MovementSpeed,
                movementRegenerationTime: mobSettings.MovementRegenerationTime,
                shootingPosition: spawnSettings.ShootingPosition);

            var navigationConfiguration = new NavigationConfiguration(
                _navigationSettings.PathBufferSize);
            var navigation = new NavigationPartial(
                navigationConfiguration,
                _navigationService,
                _logger,
                _timeScaleService);

            var mob = new ShootingMob(
                new DamagablePartial(),
                new FollowingPartial(navigation, _timeScaleService),
                _bulletService,
                _timeScaleService,
                _playerCharacter);

            mobSettings.Presenter.Init(mob);

            mob.Start(configuration);

            return mob;
        }
    }
}
