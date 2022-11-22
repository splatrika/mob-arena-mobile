using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    [RequireComponent(typeof(ShootingMobPresenter))]
    public class ShootingMobMonoSettings : MonoBehaviour,
        IReusablePresenterProvider
    {
        public ShootingMobPresenter Presenter
            => GetComponent<ShootingMobPresenter>();
        public int Health => _health;
        public int RewardPoints => _rewardPoints;
        public float BulletsSpeed => _bulletsSpeed;
        public int BulletsDamage => _bulletsDamage;
        public float GunRegenerationTime => _gunRegenerationTime;
        public float MovementSpeed => _movementSpeed;
        public float MovementRegenerationTime => _movementRegenerationTime;
        public PositionAdapter ShotPoint => _shotPoint;

        IActiveStatus IReusablePresenterProvider.Presenter => Presenter;

        [SerializeField]
        private int _health;

        [SerializeField]
        private int _rewardPoints;

        [Header("Shooting")]

        [SerializeField]
        private float _bulletsSpeed;

        [SerializeField]
        private int _bulletsDamage;

        [SerializeField]
        private float _gunRegenerationTime;

        [SerializeField]
        private PositionAdapter _shotPoint;

        [Header("Movement")]

        [SerializeField]
        private float _movementSpeed;

        [SerializeField]
        private float _movementRegenerationTime;
    }
}
