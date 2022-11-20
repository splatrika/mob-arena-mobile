using System.Collections.Generic;
using System.Linq;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class WalkingMobServiceFactory : IFactory<WalkingMobService>
    {
        private LevelMonoSettings _levelMonoSettings;
        private WalkingMobDatabaseSettings _kindsDatabase;
        private readonly IPlayerCharacter _playerCharacter;
        private readonly ITimeScaleService _timeScaleService;
        private readonly ILogger _logger;
        private readonly INavigationService _navigationService;
        private readonly NavigationSettings _navigationSettings;


        public WalkingMobServiceFactory(
            LevelMonoSettings levelMonoSettings,
            WalkingMobDatabaseSettings kindsDatabase,
            IPlayerCharacter playerCharacter,
            ITimeScaleService timeScaleService,
            ILogger logger,
            INavigationService navigationService,
            NavigationSettings navigationSettings)
        {
            _levelMonoSettings = levelMonoSettings;
            _kindsDatabase = kindsDatabase;
            _playerCharacter = playerCharacter;
            _timeScaleService = timeScaleService;
            _logger = logger;
            _navigationService = navigationService;
            _navigationSettings = navigationSettings;
        }


        public WalkingMobService Create()
        {
            var modelsCount = 0;
            var kindsCount = new Dictionary<int, int>();

            foreach (var wave in _levelMonoSettings.Waves)
            {
                foreach (var point in wave.GetSpawnPoints())
                {
                    if (point is WalkingMobSpawnerMonoSettings p)
                    {
                        modelsCount++;
                        if (!kindsCount.ContainsKey(p.MobKind))
                        {
                            kindsCount.Add(p.MobKind, 0);
                        }
                        kindsCount[p.MobKind]++;
                    }
                }
            }

            foreach (var kind in _kindsDatabase.Kinds)
            {
                if (!kindsCount.ContainsKey(kind.KindId))
                {
                    kindsCount.Add(kind.KindId, 0);
                }
            }

            var configuration = new WalkingMobServiceConfiguration(
                presenterPoolsSizes: kindsCount,
                mobKinds: _kindsDatabase.Kinds.ToArray(),
                modelPoolSize: modelsCount);

            return new WalkingMobService(configuration, _playerCharacter,
                _timeScaleService, _logger, _navigationService,
                _navigationSettings);
        }
    }
}
