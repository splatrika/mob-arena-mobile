using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class WalkingMobService : MobService<WalkingMobConfiguration,
        WalkingMob, WalkingMobPool, WalkingMobMonoSettings>,
        IWalkingMobService, IUpdatable
    {
        private readonly IPlayerCharacter _playerCharacter;
        private readonly ITimeScaleService _timeScaleService;
        private readonly ILogger _logger;
        private readonly INavigationService _navigationService;
        private readonly NavigationSettings _navigationSettings;


        public WalkingMobService(
            WalkingMobServiceConfiguration configuration,
            IPlayerCharacter playerCharacter,
            ITimeScaleService timeScaleService,
            ILogger logger,
            INavigationService navigationService,
            NavigationSettings navigationSettings)
            : base(configuration, logger)
        {
            _playerCharacter = playerCharacter;
            _timeScaleService = timeScaleService;
            _logger = logger;
            _navigationService = navigationService;
            _navigationSettings = navigationSettings;

            AssignModelPool(configuration);
        }


        public IHealth Spawn(int kindId, Vector3 position)
        {
            var monoSettings = GetMonoSettings(kindId);

            var mobConfiguration = new WalkingMobConfiguration(
                startPoint: position,
                speed: monoSettings.Speed,
                atackDistance: monoSettings.AtackDistance,
                atackRegenerationTime: monoSettings.AtackRegenerationTime,
                atackDamage: monoSettings.AtackDamage,
                health: monoSettings.Health,
                walkingRegenerationTime: monoSettings.WalkingRegenerationTime,
                rewardPoints: monoSettings.RewardPoints);

            ModelPool.Spawn(mobConfiguration, out var mob);
            monoSettings.Presenter.Init(mob);

            return mob;
        }


        public void Update(float deltaTime)
        {
            foreach (var mob in ModelPool.Objects)
            {
                mob.Update(deltaTime);
            }
        }


        protected override WalkingMobPool CreateModelPool(int poolSize)
        {
            return new WalkingMobPool(
                poolSize,
                _playerCharacter,
                _timeScaleService,
                _logger,
                _navigationService,
                _navigationSettings);
        }
    }
}
