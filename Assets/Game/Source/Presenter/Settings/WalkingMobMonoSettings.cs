using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    [RequireComponent(typeof(WalkingMobPresenter))]
    public class WalkingMobMonoSettings : MonoBehaviour,
        IReusablePresenterProvider
    {
        public WalkingMobPresenter Presenter
            => GetComponent<WalkingMobPresenter>();

        public int Health => _health;
        public int RewardPoints => _rewardPoints;
        public float Speed => _speed;
        public float WalkingRegenerationTime => _walkingRegenerationTime;
        public float AttackDistance => _attackDistance;
        public float AttackRegenerationTime => _attackRegenerationTime;
        public int AttackDamage => _attackDamage;

        IActiveStatus IReusablePresenterProvider.Presenter => Presenter;

        [SerializeField]
        private int _health;

        [SerializeField]
        private int _rewardPoints;

        [Header("Movement")]

        [SerializeField]
        private float _speed;

        [SerializeField]
        private float _walkingRegenerationTime;

        [Header("Attack")]

        [SerializeField]
        private float _attackDistance;

        [SerializeField]
        private float _attackRegenerationTime;

        [SerializeField]
        private int _attackDamage;


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _attackDistance);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position,
                transform.position + Vector3.forward * _speed);
        }
    }
}
