using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IFollowingStrategy : IUpdatable
    {
        Vector3 Position { get; }

        void SetPosition(Vector3 position);
        void Stop();

        event Action Arrived;
        event Action StartedFollowing;
        event Action<Vector3> DirectionUpdated;
    }
}