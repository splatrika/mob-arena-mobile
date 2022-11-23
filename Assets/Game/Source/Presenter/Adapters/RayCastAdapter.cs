using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class RaycastAdapter : IRaycastService
    {
        public const int BufferSize = 10;

        private RaycastHit[] _buffer;


        public RaycastAdapter()
        {
            _buffer = new RaycastHit[BufferSize];
        }


        public bool Raycast(Vector3 from, Vector3 to, int layerMask,
            out object hittedModel)
        {
            var direction = to - from;
            var lenght = direction.magnitude;
            var ray = new Ray(from, direction.normalized);
            var count = Physics.RaycastNonAlloc(ray, _buffer, lenght,
                layerMask);
            if (count > 0)
            {
                var hit = _buffer[0];
                IPresenter presenter = null;
                var isPresenter = hit.rigidbody != null &&
                    hit.rigidbody.TryGetComponent(out presenter);
                if (isPresenter)
                {
                    hittedModel = presenter.Model;
                    return true;
                }
            }
            hittedModel = null;
            return false;
        }
    }
}
