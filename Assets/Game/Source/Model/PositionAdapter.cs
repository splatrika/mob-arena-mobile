using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class PositionAdapter : IPositionProvider
    {
        public Vector3 Position => _getter.Invoke();

        private Func<Vector3> _getter;


        public PositionAdapter(Func<Vector3> getter)
        {
            _getter = getter;
        }
    }
}
