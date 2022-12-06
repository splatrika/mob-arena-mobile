using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Settings
{
    public abstract class MobSpawnPointMonoSettings : MonoBehaviour
    {
        public float SpawnTime => _spawnTime;

        [SerializeField]
        private float _spawnTime;

        public abstract void DrawGizmos();
        public abstract MobSpawnPoint GetSpawnPoint(DiContainer container);
    }
}
