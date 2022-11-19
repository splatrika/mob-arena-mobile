using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    public abstract class MobSpawnPointMonoSettings : MonoBehaviour
    {
        public float SpawnTime => _spawnTime;

        [SerializeField]
        private float _spawnTime;

        public abstract void DrawGizmos();
    }
}
