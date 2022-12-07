using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class ShootingMobSpawnPoint : MobSpawnPoint
    {
        public int KindId { get; }
        public Vector3 SpawnPosition { get; }
        public Vector3 ShootingPosition { get; }

        private readonly IShootingMobService _shootingMobService;


        public ShootingMobSpawnPoint(
            float spawnTime,
            int kindId,
            Vector3 spawnPosition,
            Vector3 shootingPosition,
            IShootingMobService shootingMobService)
            : base(spawnTime)
        {
            KindId = kindId;
            SpawnPosition = spawnPosition;
            ShootingPosition = shootingPosition;

            _shootingMobService = shootingMobService;
        }


        public override IDamageable Spawn()
        {
            return _shootingMobService.Spawn(KindId, SpawnPosition,
                ShootingPosition);
        }
    }
}
