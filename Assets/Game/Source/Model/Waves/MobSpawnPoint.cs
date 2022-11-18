namespace Splatrika.MobArenaMobile.Model
{
    public abstract class MobSpawnPoint
    {
        public float SpawnTime { get; }


        protected MobSpawnPoint(float spawnTime)
        {
            SpawnTime = spawnTime;
        }


        public abstract IHealth Spawn();
    }
}
