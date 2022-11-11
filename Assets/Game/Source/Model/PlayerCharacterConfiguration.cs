using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class PlayerCharacterConfiguration
    {
        public int Health;
        public float ShootRegenerationTime;
        public Vector3 Position;
        public Vector3 Direction;

        public PlayerCharacterConfiguration(
            int health,
            float shootRegenerationTime,
            Vector3 position,
            Vector3 direction)
        {
            Health = health;
            ShootRegenerationTime = shootRegenerationTime;
            Position = position;
            Direction = direction;
        }
    }
}
