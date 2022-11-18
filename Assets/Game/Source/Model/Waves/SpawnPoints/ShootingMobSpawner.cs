using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class ShootingMobSpawner : MobSpawnPoint
    {
        public int KindId { get; }
        public Vector3 SpawnPosition { get; }
        public Vector3 ShootingPosition { get; }

        private readonly IShootingMobService _shootingMobService;


        public ShootingMobSpawner(ShootingMobSpawnerConfiguration configuration,
            IShootingMobService shootingMobService)
            : base(configuration.SpawnTime)
        {
            KindId = configuration.KindId;
            SpawnPosition = configuration.SpawnPosition;
            ShootingPosition = configuration.ShootingPosition;

            _shootingMobService = shootingMobService;
        }


        public override IHealth Spawn()
        {
            return _shootingMobService.Spawn(KindId, SpawnPosition,
                ShootingPosition);
        }
    }
}
