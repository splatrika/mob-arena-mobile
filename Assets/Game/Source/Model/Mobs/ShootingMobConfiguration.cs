using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class ShootingMobConfiguration : MobConfiguration
    {
        public float Speed;
        public Vector3 ShootingPosition;
        public IPositionProvider ShotPoint;
        public float BulletsSpeed;
        public int BulletsDamage;
        public float GunRegenerationTime;


        public ShootingMobConfiguration(
            int health,
            Vector3 position,
            int rewardPoints,
            float walkingRegenerationTime,
            float speed,
            float bulletsSpeed,
            int bulletsDamage,
            float gunRegenerationTime,
            IPositionProvider shotPoint,
            Vector3 shootingPosition)
            : base(health, position, rewardPoints, walkingRegenerationTime)
        {
            Speed = speed;
            BulletsSpeed = bulletsSpeed;
            BulletsDamage = bulletsDamage;
            GunRegenerationTime = gunRegenerationTime;
            ShotPoint = shotPoint;
            ShootingPosition = shootingPosition;
        }
    }
}
