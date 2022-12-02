using System;
using System.Collections;
using System.Collections.Generic;
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


        public LevelFactory(
            IPlayerCharacter playerCharacter,
            ILogger logger,
            ITimeScaleService timeScaleService)
        {
            _playerCharacter = playerCharacter;
            _logger = logger;
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

            var waves = new WaveConfiguration[0];
            // todo parse waves

            var configuration = new LevelConfiguration(
                waves, settings.TimeScaleAcceleration);

            return new Level(configuration, _playerCharacter,
                _timeScaleService);
        }
    }
}
