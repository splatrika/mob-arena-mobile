using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public struct ShootingMobConfiguration
    {
        public int Health;
        public float BulletsSpeed;
        public int BulletsDamage;
        public float GunRegenerationTime;
        public Vector3 Start;
        public float MovementSpeed;
        public float MovementRegenerationTime;
        public Vector3 ShootingPosition;
        public IPositionProvider ShotPoint;
        public int RewardPoints;


        public ShootingMobConfiguration(
            int health,
            float bulletsSpeed,
            int bulletsDamage,
            float gunRegenerationTime,
            Vector3 start,
            float movementSpeed,
            float movementRegenerationTime,
            Vector3 shootingPosition,
            IPositionProvider shotPoint,
            int rewardPoints)
        {
            Health = health;
            BulletsSpeed = bulletsSpeed;
            BulletsDamage = bulletsDamage;
            GunRegenerationTime = gunRegenerationTime;
            Start = start;
            MovementSpeed = movementSpeed;
            MovementRegenerationTime = movementRegenerationTime;
            ShootingPosition = shootingPosition;
            ShotPoint = shotPoint;
            RewardPoints = rewardPoints;
        }
    }
}
