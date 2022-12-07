using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IFollowingStrategy : IUpdatable
    {
        Vector3 Position { get; }

        void SetRegenerationTime(float time);
        void SetPosition(Vector3 position);
        void Stop();
        void Hit();

        event Action Arrived;
        event Action StartedFollowing;
        event Action<Vector3> DirectionUpdated;
        event Action Frozen;
        event Action Resumed;
    }
}