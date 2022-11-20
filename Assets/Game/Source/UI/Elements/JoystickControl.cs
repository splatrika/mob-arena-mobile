using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.UI
{
    public abstract class JoystickControl : MonoBehaviour
    {
        public abstract Vector2 Offset { get; }

        public abstract event Action Touched;
        public abstract event Action Untouched;
        public abstract event Action<Vector2> OffsetUpdated;
    }
}
