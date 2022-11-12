using System;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class PlayerAtSpawnPointFactory : IFactory<PlayerCharacter>
    {
        private readonly PlayerSettings _settings;
        private readonly IFriendBulletService _friendBulletService;
        private readonly ITimeScaleService _timeScaleService;
        private readonly ILogger _logger;
        private readonly PlayerSpawnPoint _spawnPoint;


        public PlayerAtSpawnPointFactory(
            PlayerSettings settings,
            IFriendBulletService friendBulletService,
            ITimeScaleService timeScaleService,
            ILogger logger)
        {
            _settings = settings;
            _friendBulletService = friendBulletService;
            _timeScaleService = timeScaleService;
            _logger = logger;

            _spawnPoint = GameObject.FindObjectOfType<PlayerSpawnPoint>();
            if (!_spawnPoint)
            {
                throw new NullReferenceException(
                    "PlayerSpawnPoint is not placed on the scene");
            }
        }


        public PlayerCharacter Create()
        {
            var configuration = new PlayerCharacterConfiguration(
                health: _settings.DefaultHealth,
                shootRegenerationTime: _settings.ShootRegenerationTime,
                position: _spawnPoint.transform.position,
                direction: Vector3.forward);

            var character = new PlayerCharacter(
                configuration,
                _friendBulletService,
                _timeScaleService,
                _logger);

            var presenter = GameObject.Instantiate(_settings.Prefab);
            presenter.Init(character);

            return character;
        }
    }
}
