using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public struct BulletConfiguration
    {
        public Vector3 Position;
        public Vector3 Direction;
        public float Speed;
        public int Damage;


        public BulletConfiguration(
            Vector3 position,
            Vector3 direction,
            float speed,
            int damage)
        {
            Position = position;
            Direction = direction;
            Speed = speed;
            Damage = damage;
        }
    }
}
