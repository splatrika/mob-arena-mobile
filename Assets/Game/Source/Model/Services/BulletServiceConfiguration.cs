namespace Splatrika.MobArenaMobile.Model
{
    public class BulletServiceConfiguration
    {
        public int PoolSize;
        public int LayerMask;
        public float BulletsLifeTime;


        public BulletServiceConfiguration(
            int poolSize,
            int layerMask,
            float bulletsLifeTime)
        {
            PoolSize = poolSize;
            LayerMask = layerMask;
            BulletsLifeTime = bulletsLifeTime;
        }
    }
}
