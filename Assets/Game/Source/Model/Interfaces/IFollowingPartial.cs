using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IFollowingPartial: IUpdatable
    {
        Vector3 Position { get; }

        event Action Arrived;
        event Action<Vector3> MovementStarted;
        event Action MovementStopped;

        void Start(FollowingConfiguration configuration);

        void Hit();
    }
}
