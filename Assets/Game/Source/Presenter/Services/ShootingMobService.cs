using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class ShootingMobService : MobService<ShootingMobConfiguration,
        ShootingMob, ShootingMobPool, ShootingMobMonoSettings>,
        IShootingMobService, IUpdatable
    {
        private readonly ILogger _logger;
        private readonly IBulletService _bulletService;
        private readonly ITimeScaleService _timeScaleService;
        private readonly IPlayerCharacter _playerCharacter;
        private readonly INavigationService _navigationService;
        private readonly NavigationSettings _navigationSettings;


        public ShootingMobService(
            ShootingMobServiceConfiguration configuration,
            ILogger logger,
            IBulletService bulletService,
            ITimeScaleService timeScaleService,
            IPlayerCharacter playerCharacter,
            INavigationService navigationService,
            NavigationSettings navigationSettings)
            : base(configuration, logger)
        {
            _logger = logger;
            _bulletService = bulletService;
            _timeScaleService = timeScaleService;
            _playerCharacter = playerCharacter;
            _navigationService = navigationService;
            _navigationSettings = navigationSettings;

            AssignModelPool(configuration);
        }


        public void Update(float deltaTime)
        {
            foreach (var mob in ModelPool.Objects)
            {
                mob.Update(deltaTime);
            }
        }


        protected sealed override ShootingMobPool CreateModelPool(int size)
        {
            return new ShootingMobPool(size, _bulletService,
                _timeScaleService, _playerCharacter, _logger,
                _navigationService, _navigationSettings);
        }


        public IHealth Spawn(int kindId, Vector3 position,
            Vector3 shootingPosition)
        {
            var monoSettings = GetMonoSettings(kindId);

            var mobConfigration = new ShootingMobConfiguration(
                health: monoSettings.Health,
                bulletsSpeed: monoSettings.BulletsSpeed,
                bulletsDamage: monoSettings.BulletsDamage,
                gunRegenerationTime: monoSettings.GunRegenerationTime,
                start: position,
                movementSpeed: monoSettings.MovementSpeed,
                movementRegenerationTime: monoSettings.MovementRegenerationTime,
                shootingPosition: shootingPosition,
                shotPoint: monoSettings.ShotPoint,
                rewardPoints: monoSettings.RewardPoints);

            ModelPool.Spawn(mobConfigration, out var mob);

            monoSettings.Presenter.Init(mob);

            return mob;
        }

    }
}
