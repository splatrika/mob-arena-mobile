using UnityEngine;

namespace Splatrika.MobArenaMobile.Settings
{
    [AddComponentMenu("MobArenaMobile/WaveMonoSettings")]
    public class WaveMonoSettings : MonoBehaviour
    {
        public MobSpawnPointMonoSettings[] GetSpawnPoints()
        {
            return GetComponentsInChildren<MobSpawnPointMonoSettings>();
        }


        private void OnDrawGizmos()
        {
            var spawnPoints = GetSpawnPoints();
            foreach (var spawnPoint in spawnPoints)
            {
                spawnPoint.DrawGizmos();
            }
        }
    }
}
