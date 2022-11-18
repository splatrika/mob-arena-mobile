using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public struct WalkingMobSpawnerConfiguration
    {
        public float SpawnTime;
        public int KindId;
        public Vector3 Position;


        public WalkingMobSpawnerConfiguration(
            float spawnTime,
            int kindId,
            Vector3 position)
        {
            SpawnTime = spawnTime;
            KindId = kindId;
            Position = position;
        }
    }
}
