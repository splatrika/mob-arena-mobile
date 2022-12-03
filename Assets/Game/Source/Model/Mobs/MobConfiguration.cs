using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class MobConfiguration
    {
        public int Health;
        public Vector3 Position;
        public int RewardPoints;


        public MobConfiguration(int health, Vector3 position, int rewardPoints)
        {
            Health = health;
            Position = position;
            RewardPoints = rewardPoints;
        }
    }
}
