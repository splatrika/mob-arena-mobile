namespace Splatrika.MobArenaMobile.Factories
{
    public class KindConfiguration<T>
    {
        public int KindId;
        public T Prefab;
        public int PoolSize;


        public KindConfiguration(int kindId, T prefab, int poolSize)
        {
            KindId = kindId;
            Prefab = prefab;
            PoolSize = poolSize;
        }
    }
}
