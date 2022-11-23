using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    [CreateAssetMenu(menuName = "MobArenaMobile/Settings/Player")]
    public class PlayerSettings : ScriptableObject
    {
        public PlayerCharacterMonoSettings Prefab => _prefab;
        public int DefaultHealth => _defaultHealth;
        public float ShootRegenerationTime => _shootRegenerationTime;
        public int BulletDamage => _bulletDamage;
        public float BulletSpeed => _bulletSpeed;
        public string BulletPrefab => _bulletPrefab;
        public int BulletsLayerMask => _bulletsLayerMask.value;

        [SerializeField]
        private PlayerCharacterMonoSettings _prefab;

        [SerializeField]
        private int _defaultHealth;

        [SerializeField]
        private float _shootRegenerationTime;

        [Header("Bullets")]

        [SerializeField]
        private int _bulletDamage;

        [SerializeField]
        private float _bulletSpeed;

        [SerializeField]
        private string _bulletPrefab;

        [SerializeField]
        private LayerMask _bulletsLayerMask;


        private void OnValidate()
        {
            if (!string.IsNullOrEmpty(_bulletPrefab))
            {
                var bulletPrefab = Resources.Load<BulletPresenter>(_bulletPrefab);
                if (!bulletPrefab)
                {
                    _bulletPrefab = "";
                }
            }
        }
    }
}
