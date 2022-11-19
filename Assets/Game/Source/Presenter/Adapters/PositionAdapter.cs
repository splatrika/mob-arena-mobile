using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class PositionAdapter : MonoBehaviour, IPositionProvider
    {
        public Vector3 Position => GetPosition();

        private Transform _transform;


        public Vector3 GetPosition()
        {
            if (!_transform)
            {
                _transform = transform;
            }
            return _transform.position;
        }
    }
}
