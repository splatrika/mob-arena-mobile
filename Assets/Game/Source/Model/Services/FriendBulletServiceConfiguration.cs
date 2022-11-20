namespace Splatrika.MobArenaMobile.Model
{
    public class FriendBulletServiceConfiguration : BulletServiceConfiguration
    {
        public float Speed;
        public int Damage;


        public FriendBulletServiceConfiguration(
            int poolSize,
            int layerMask,
            float bulletsLifeTime,
            float speed,
            int damage)
            : base(poolSize, layerMask, bulletsLifeTime)
        {
            Speed = speed;
            Damage = damage;
        }
    }
}
