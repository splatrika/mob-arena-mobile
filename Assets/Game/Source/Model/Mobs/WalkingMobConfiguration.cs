using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class WalkingMobConfiguration
    {
        public Vector3 StartPoint;
        public float Speed;
        public float AtackDistance;
        public float AtackRegenerationTime;
        public int Health;
        public float WalkingRegenerationTime;

        public WalkingMobConfiguration(
            float speed,
            float atackDistance,
            int health,
            float walkingRegenerationTime)
        {
            Speed = speed;
            AtackDistance = atackDistance;
            Health = health;
            WalkingRegenerationTime = walkingRegenerationTime;
        }
    }
}
