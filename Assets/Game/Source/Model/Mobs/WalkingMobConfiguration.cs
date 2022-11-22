using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class WalkingMobConfiguration
    {
        public Vector3 StartPoint;
        public float Speed;
        public float AtackDistance;
        public float AtackRegenerationTime;
        public int AtackDamage;
        public int Health;
        public float WalkingRegenerationTime;
        public int RewardPoints;

        public WalkingMobConfiguration(
            Vector3 startPoint,
            float speed,
            float atackDistance,
            float atackRegenerationTime,
            int atackDamage,
            int health,
            float walkingRegenerationTime,
            int rewardPoints)
        {
            StartPoint = startPoint;
            Speed = speed;
            AtackDistance = atackDistance;
            AtackRegenerationTime = atackRegenerationTime;
            AtackDamage = atackDamage;
            Health = health;
            WalkingRegenerationTime = walkingRegenerationTime;
            RewardPoints = rewardPoints;
        }
    }
}
