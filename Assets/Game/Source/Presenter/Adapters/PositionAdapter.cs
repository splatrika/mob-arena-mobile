using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class PositionAdapter : MonoBehaviour, IPositionProvider
    {
        public Vector3 Position => _transform.position;

        private Transform _transform;


        public void Init(Transform point)
        {
            _transform = point;
        }
    }
}
