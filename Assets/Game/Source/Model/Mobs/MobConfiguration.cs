using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class MobConfiguration
    {
        public int Health;
        public Vector3 Position;
        public int RewardPoints;
        public float WalkingRegenerationTime;


        public MobConfiguration(
            int health,
            Vector3 position,
            int rewardPoints,
            float walkingRegenerationTime)
        {
            Health = health;
            Position = position;
            RewardPoints = rewardPoints;
            WalkingRegenerationTime = walkingRegenerationTime;
        }
    }
}
