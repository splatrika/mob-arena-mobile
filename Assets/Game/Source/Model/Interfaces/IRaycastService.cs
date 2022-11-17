using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IRaycastService
    {
        bool Raycast(Vector3 from, Vector3 to, int layerMask,
            out object hittedModel);
    }
}
