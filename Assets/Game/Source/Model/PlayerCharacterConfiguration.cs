using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class PlayerCharacterConfiguration
    {
        public int Health;
        public float ShootRegenerationTime;
        public Vector3 Position;
        public Vector3 Direction;
        public Vector3 CenterOffset;


        public PlayerCharacterConfiguration(
            int health,
            float shootRegenerationTime,
            Vector3 position,
            Vector3 direction,
            Vector3 centerOffset)
        {
            Health = health;
            ShootRegenerationTime = shootRegenerationTime;
            Position = position;
            Direction = direction;
            CenterOffset = centerOffset;
        }
    }
}
