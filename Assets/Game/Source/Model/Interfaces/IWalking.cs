using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IWalking
    {
        bool IsWalking { get; }
        Vector3 Direction { get; }

        event Action MovementStarted;
        event Action MovementStopped;
        event Action<Vector3> DirectionUpdated;
    }
}
