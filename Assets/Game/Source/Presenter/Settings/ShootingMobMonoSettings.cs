using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    [RequireComponent(typeof(ShootingMobPresenter))]
    public class ShootingMobMonoSettings : MonoBehaviour
    {
        public ShootingMobPresenter Presenter
            => GetComponent<ShootingMobPresenter>();
        public int Health => _health;
        public float BulletsSpeed => _bulletsSpeed;
        public int BulletsDamage => _bulletsDamage;
        public float GunRegenerationTime => _gunRegenerationTime;
        public float MovementSpeed => _movementSpeed;
        public float MovementRegenerationTime => _movementRegenerationTime;


        [SerializeField]
        private int _health;

        [Header("Shooting")]

        [SerializeField]
        private float _bulletsSpeed;

        [SerializeField]
        private int _bulletsDamage;

        [SerializeField]
        private float _gunRegenerationTime;

        [Header("Movement")]

        [SerializeField]
        private float _movementSpeed;

        [SerializeField]
        private float _movementRegenerationTime;
    }
}
