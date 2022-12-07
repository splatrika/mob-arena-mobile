using System;
using System.Linq;
using Splatrika.MobArenaMobile.Model;
using Splatrika.MobArenaMobile.Settings;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Factories
{
    public class LevelFactory : IFactory<Level>
    {
        private readonly IPlayerCharacter _playerCharacter;
        private readonly ILogger _logger;
        private readonly ITimeScaleService _timeScaleService;
        private readonly DiContainer _container;


        public LevelFactory(
            IPlayerCharacter playerCharacter,
            ILogger logger,
            ITimeScaleService timeScaleService,
            DiContainer container)
        {
            _playerCharacter = playerCharacter;
            _logger = logger;
            _timeScaleService = timeScaleService;
            _container = container;
        }


        public Level Create()
        {
            var levelSettings = GameObject.FindObjectOfType<LevelMonoSettings>();
            if (!levelSettings)
            {
                throw new NullReferenceException(
                    "There is no LevelMonoSettings found on the scene");
            }

            var wavesSettings = levelSettings.Waves;
            var waves = new WaveConfiguration[wavesSettings.Count];
            for (int i = 0; i < wavesSettings.Count; i++)
            {
                var item = wavesSettings[i];
                var spawnPoints = item.GetSpawnPoints()
                    .Select(x => x.GetSpawnPoint(_container))
                    .ToArray();
                waves[i] = new WaveConfiguration(spawnPoints);
            }

            var configuration = new LevelConfiguration(
                waves, levelSettings.TimeScaleAcceleration);

            return new Level(configuration, _playerCharacter,
                _timeScaleService);
        }
    }
}
