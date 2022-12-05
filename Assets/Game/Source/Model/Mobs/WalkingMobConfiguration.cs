using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class WalkingMobConfiguration : MobConfiguration
    {
        public float Speed;
        public float AttackDistance;
        public int Damage;
        public float AttackRegenerationTime;

        public WalkingMobConfiguration(
            int health,
            Vector3 position,
            int rewardPoints,
            float speed,
            float attackDistance,
            int damage,
            float attackRegenerationTime)
            : base(health, position, rewardPoints)
        {
            Speed = speed;
            AttackDistance = attackDistance;
            Damage = damage;
            AttackRegenerationTime = attackRegenerationTime;
        }
    }
}
