using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IFollowingPartial: IUpdatable
    {
        Vector3 Position { get; }
        bool IsMoving { get; }

        event Action Arrived;
        event Action MovementStarted;
        event Action<Vector3> DirectionUpdated;
        event Action MovementStopped;

        void Start(FollowingConfiguration configuration);

        void Hit();
    }
}
