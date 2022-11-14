using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public struct FollowingConfiguration
    {
        public Vector3 Start;
        public Vector3 Target;
        public float MinDistance;
        public float Speed;
        public float RegenerationTime;

        public FollowingConfiguration(
            Vector3 startPoint,
            Vector3 followTarget,
            float minDistance,
            float speed,
            float regenerationTime)
        {
            Start = startPoint;
            Target = followTarget;
            MinDistance = minDistance;
            Speed = speed;
            RegenerationTime = regenerationTime;
        }
    }
}
