using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Settings
{
    public class WalkingMobSpawnPointMonoSettngs : MobSpawnPointMonoSettings
    {
        public override void DrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.25f);
        }


        public override MobSpawnPoint GetSpawnPoint(DiContainer container)
        {
            var service = container.Resolve<IWalkingMobService>();
            return new WalkingMobSpawnPoint(SpawnTime, MobKind,
                transform.position, service);
        }
    }
}
