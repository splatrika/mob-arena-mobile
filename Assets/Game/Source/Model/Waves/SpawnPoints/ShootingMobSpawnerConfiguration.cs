using UnityEngine;


namespace Splatrika.MobArenaMobile.Model
{
    public struct ShootingMobSpawnerConfiguration
    {
        public float SpawnTime;
        public int KindId;
        public Vector3 SpawnPosition;
        public Vector3 ShootingPosition;


        public ShootingMobSpawnerConfiguration(
            float spawnTime,
            int kindId,
            Vector3 spawnPosition,
            Vector3 shootingPosition)
        {
            SpawnTime = spawnTime;
            KindId = kindId;
            SpawnPosition = spawnPosition;
            ShootingPosition = shootingPosition;
        }
    }
}
