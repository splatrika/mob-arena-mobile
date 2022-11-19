using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    [AddComponentMenu("MobArenaMobile/MobSpawnPoints/WalkingMobSpawnerMonoSettings")]
    public class WalkingMobSpawnerMonoSettings : MobSpawnPointMonoSettings
    {
        public int MobKind => _mobKind;

        [SerializeField]
        private int _mobKind;


        public sealed override void DrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.25f);
        }
    }
}
