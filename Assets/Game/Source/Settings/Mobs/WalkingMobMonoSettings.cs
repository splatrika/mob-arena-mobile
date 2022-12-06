using Splatrika.MobArenaMobile.Presenter;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Settings
{
    [RequireComponent(typeof(WalkingMobPresenter))]
    public class WalkingMobMonoSettings : MonoBehaviour
    {
        public WalkingMobPresenter Presenter
            => GetComponent<WalkingMobPresenter>();

        public int Health => _health;
        public int RewardPoints => _rewardPoints;
        public float Speed => _speed;
        public float WalkingRegenerationTime => _walkingRegenerationTime;
        public float AttackDistance => _atackDistance;
        public float AttackRegenerationTime => _atackRegenerationTime;
        public int AttackDamage => _atackDamage;

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
