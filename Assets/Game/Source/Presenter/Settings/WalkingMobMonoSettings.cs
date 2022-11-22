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
        public float AtackDistance => _atackDistance;
        public float AtackRegenerationTime => _atackRegenerationTime;
        public int AtackDamage => _atackDamage;

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

        [Header("Atack")]

        [SerializeField]
        private float _atackDistance;

        [SerializeField]
        private float _atackRegenerationTime;

        [SerializeField]
        private int _atackDamage;


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _atackDistance);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position,
                transform.position + Vector3.forward * _speed);
        }
    }
}
