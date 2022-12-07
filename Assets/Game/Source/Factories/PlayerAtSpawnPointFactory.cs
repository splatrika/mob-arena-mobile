using System;
using Splatrika.MobArenaMobile.Model;
using Splatrika.MobArenaMobile.Presenter;
using Splatrika.MobArenaMobile.Settings;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Factories
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
            var prefab = _settings.Prefab;

            if (!prefab)
            {
                _logger.LogError(nameof(PlayerAtSpawnPointFactory),
                    "PlayerCharacter prefab is not assigned in PlayerSettings");
                return null;
            }

            if (!prefab.Center)
            {
                _logger.LogError(nameof(PlayerAtSpawnPointFactory),
                    "Center is not assigned in PlayerCharacter prefab");
                return null;
            }

            var centerOffset = prefab.Center.position
                - prefab.transform.position;

            var configuration = new PlayerCharacterConfiguration(
                health: _settings.DefaultHealth,
                shootRegenerationTime: _settings.ShootRegenerationTime,
                position: _spawnPoint.transform.position,
                direction: Vector3.forward,
                centerOffset: centerOffset);

            var damageable = new DamageablePartial();

            var character = new PlayerCharacter(
                configuration,
                _friendBulletService,
                _timeScaleService,
                _logger,
                damageable);

            var presenter = GameObject
                .Instantiate(_settings.Prefab)
                .Presenter;
            presenter.Init(character);

            return character;
        }
    }
}
