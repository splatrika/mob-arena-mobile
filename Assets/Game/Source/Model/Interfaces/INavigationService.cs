using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public interface INavigationService
    {
        bool TryGetPath(Vector3 start, Vector3 target,
            Vector3[] pathBuffer, out int points);
    }
}
