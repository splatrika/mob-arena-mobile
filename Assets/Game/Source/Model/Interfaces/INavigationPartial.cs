using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public interface INavigationPartial: IUpdatable
    {
        Vector3 Position { get; }

        bool Start(Vector3 start, Vector3 target, float speed);
        void Stop();
    }
}
