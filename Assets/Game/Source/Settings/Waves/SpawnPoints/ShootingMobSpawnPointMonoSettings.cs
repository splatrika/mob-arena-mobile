using System.Collections;
using System.Collections.Generic;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Settings
{
    public class ShootingMobSpawnPointMonoSettings : MobSpawnPointMonoSettings
    {
        [SerializeField]
        private Transform _shootingPosition;


        public override void DrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.25f);

            if (_shootingPosition)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, _shootingPosition.position);
                Gizmos.DrawSphere(_shootingPosition.position, 0.125f);
            }
            if (!_shootingPosition)
            {
                Gizmos.DrawIcon(transform.position, "console.erroricon");
            }
        }


        public override MobSpawnPoint GetSpawnPoint(DiContainer container)
        {
            var service = container.Resolve<IShootingMobService>();
            return new ShootingMobSpawnPoint(SpawnTime, MobKind,
                transform.position, _shootingPosition.position,
                service);
        }


        private void OnValidate()
        {
            if (!_shootingPosition)
            {
                Debug.LogError("Shooting Position is not assigned");
            }
        }
    }
}
