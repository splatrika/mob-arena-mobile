using Splatrika.MobArenaMobile.Presenter;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Settings
{
    [RequireComponent(typeof(PlayerCharacterPresenter))]
    public class PlayerCharacterMonoSettings : MonoBehaviour
    {
        public PlayerCharacterPresenter Presenter =>
            GetComponent<PlayerCharacterPresenter>();
        public Transform Center => _center;

        [SerializeField]
        private Transform _center;
    }
}
