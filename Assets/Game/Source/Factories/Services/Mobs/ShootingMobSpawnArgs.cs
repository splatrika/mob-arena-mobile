using UnityEngine;

namespace Splatrika.MobArenaMobile.Factories
{
    public struct ShootingMobSpawnArgs
    {
        public Vector3 Position;
        public Vector3 ShootingPosition;


        public ShootingMobSpawnArgs(Vector3 position, Vector3 shootingPosition)
        {
            Position = position;
            ShootingPosition = shootingPosition;
        }
    }
}
