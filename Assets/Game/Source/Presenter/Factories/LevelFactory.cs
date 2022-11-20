using System;
using System.Collections;
using System.Collections.Generic;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class LevelFactory : IFactory<Level>
    {
        private readonly IPlayerCharacter _playerCharacter;
        private readonly ILogger _logger;
        private readonly List<IMobSpawnPointFactory> _spawnPointFactories;
        private readonly ITimeScaleService _timeScaleService;


        public LevelFactory(
            IPlayerCharacter playerCharacter,
            ILogger logger,
            List<IMobSpawnPointFactory> spawnPointFactories,
            ITimeScaleService timeScaleService)
        {
            _playerCharacter = playerCharacter;
            _logger = logger;
            _spawnPointFactories = spawnPointFactories;
            _timeScaleService = timeScaleService;
        }


        public Level Create()
        {
            var settings = GameObject.FindObjectOfType<LevelMonoSettings>();
            if (!settings)
            {
                throw new NullReferenceException(
                    "There is no LevelMonoSettings found on the scene");
            }

            var waves = new WaveConfiguration[settings.Waves.Count];
            for (int i = 0; i < waves.Length; i++)
            {
                var spawnPointsSettings = settings.Waves[i].GetSpawnPoints();
                var spawnPoints = new MobSpawnPoint[spawnPointsSettings.Length];
                for (int j = 0; j < spawnPoints.Length; j++)
                {
                    spawnPoints[j] = CreateSpawnPoint(spawnPointsSettings[j]);
                }
                waves[i] = new WaveConfiguration(spawnPoints);
            }

            var configuration = new LevelConfiguration(
                waves, settings.TimeScaleAcceleration);

            return new Level(configuration, _playerCharacter,
                _timeScaleService);
        }


        private MobSpawnPoint CreateSpawnPoint(
            MobSpawnPointMonoSettings settings)
        {
            MobSpawnPoint result = null;
            foreach (var factory in _spawnPointFactories)
            {
                if (factory.CanCreateFrom(settings))
                {
                    result = factory.Create(settings);
                }
            }
            if (result == null)
            {
                _logger.LogError(nameof(LevelFactory),
                    "Unable to create mob spawn point" +
                    $"from {settings.GetType().FullName}");
            }
            return result;
        }
    }
}
