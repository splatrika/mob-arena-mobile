using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using UnityEngine.AI;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class NavigationAdapter : INavigationService
    {
        private NavMeshPath _path;


        public NavigationAdapter()
        {
            _path = new NavMeshPath();
        }


        public bool TryGetPath(Vector3 start, Vector3 target,
            Vector3[] pathBuffer, out int points)
        {
            NavMesh.CalculatePath(start, target, NavMesh.AllAreas, _path);
            for (int i = 0; i < _path.corners.Length; i++)
            {
                pathBuffer[i] = _path.corners[i];
            }
            points = _path.corners.Length;
            return _path.status == NavMeshPathStatus.PathComplete;
        }
    }
}
