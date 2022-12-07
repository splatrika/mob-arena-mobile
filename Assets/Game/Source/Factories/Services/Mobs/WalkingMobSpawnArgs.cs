using UnityEngine;

namespace Splatrika.MobArenaMobile.Factories
{
    public struct WalkingMobSpawnArgs
    {
        public Vector3 Position;


        public WalkingMobSpawnArgs(Vector3 position)
        {
            Position = position;
        }
    }
}
