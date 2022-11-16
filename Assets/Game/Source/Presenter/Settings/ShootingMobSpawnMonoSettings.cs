using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    [RequireComponent(typeof(ShootingMobMonoSettings))]
    public class ShootingMobSpawnMonoSettings : MonoBehaviour
    {
        public ShootingMobMonoSettings Settings
            => GetComponent<ShootingMobMonoSettings>();
        public Vector3 ShootingPosition => _shootingPosition.position;

        [SerializeField]
        private Transform _shootingPosition;    
    }
}
