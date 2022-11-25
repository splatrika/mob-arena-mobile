using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class WalkingMobConfiguration
    {
        public Vector3 StartPoint;
        public float Speed;
        public float AttackDistance;
        public float AttackRegenerationTime;
        public int AttackDamage;
        public int Health;
        public float WalkingRegenerationTime;
        public int RewardPoints;

        public WalkingMobConfiguration(
            Vector3 startPoint,
            float speed,
            float attackDistance,
            float attackRegenerationTime,
            int attackDamage,
            int health,
            float walkingRegenerationTime,
            int rewardPoints)
        {
            StartPoint = startPoint;
            Speed = speed;
            AttackDistance = attackDistance;
            AttackRegenerationTime = attackRegenerationTime;
            AttackDamage = attackDamage;
            Health = health;
            WalkingRegenerationTime = walkingRegenerationTime;
            RewardPoints = rewardPoints;
        }
    }
}
