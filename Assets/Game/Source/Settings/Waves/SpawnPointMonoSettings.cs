using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Settings
{
    public abstract class MobSpawnPointMonoSettings : MonoBehaviour
    {
        public float SpawnTime => _spawnTime;
        public int MobKind => _mobKind;

        [SerializeField]
        private float _spawnTime;

        [SerializeField]
        private int _mobKind;

        public abstract void DrawGizmos();
        public abstract MobSpawnPoint GetSpawnPoint(DiContainer container);
    }
}
