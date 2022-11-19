using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    [AddComponentMenu("MobArenaMobile/MobSpawnPoints/ShootingMobSpawnerMonoSettings")]
    public class ShootingMobSpawnerMonoSettings : MobSpawnPointMonoSettings
    {
        public Transform ShootingPosition => _shootingPosition;
        public int MobKind => _mobKind;

        [SerializeField]
        private Transform _shootingPosition;

        [SerializeField]
        private int _mobKind;


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
        }


        private void OnDrawGizmos()
        {
            if (!_shootingPosition)
            {
                Gizmos.DrawIcon(transform.position, "console.erroricon");
            }
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
