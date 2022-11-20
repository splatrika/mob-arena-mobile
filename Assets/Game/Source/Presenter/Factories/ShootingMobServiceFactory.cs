using System.Collections.Generic;
using System.Linq;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class ShootingMobServiceFactory : IFactory<ShootingMobService>
    {
        private readonly LevelMonoSettings _levelMonoSettings;
        private readonly ILogger _logger;
        private readonly IBulletService _bulletService;
        private readonly ITimeScaleService _timeScaleService;
        private readonly IPlayerCharacter _playerCharacter;
        private readonly INavigationService _navigationService;
        private readonly NavigationSettings _navigationSettings;
        private readonly ShootingMobDatabaseSettings _kindsDatabase;


        public ShootingMobServiceFactory(
            LevelMonoSettings levelMonoSettings,
            ILogger logger,
            IBulletService bulletService,
            ITimeScaleService timeScaleService,
            IPlayerCharacter playerCharacter,
            INavigationService navigationService,
            NavigationSettings navigationSettings,
            ShootingMobDatabaseSettings kindsDatabase)
        {
            _levelMonoSettings = levelMonoSettings;
            _logger = logger;
            _bulletService = bulletService;
            _timeScaleService = timeScaleService;
            _playerCharacter = playerCharacter;
            _navigationService = navigationService;
            _navigationSettings = navigationSettings;
            _kindsDatabase = kindsDatabase;
        }


        public ShootingMobService Create()
        {
            var modelsCount = 0;
            var kindsCount = new Dictionary<int, int>();

            foreach (var wave in _levelMonoSettings.Waves)
            {
                foreach (var point in wave.GetSpawnPoints())
                {
                    if (point is ShootingMobSpawnerMonoSettings p)
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

            var configuration = new ShootingMobServiceConfiguration(
                presenterPoolsSizes: kindsCount,
                mobKinds: _kindsDatabase.Kinds.ToArray(),
                modelPoolSize: modelsCount);

            return new ShootingMobService(configuration,
                _logger, _bulletService, _timeScaleService, _playerCharacter,
                _navigationService, _navigationSettings);
        }
    }
}
