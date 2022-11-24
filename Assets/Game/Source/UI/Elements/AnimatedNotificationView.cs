using TMPro;
using UnityEngine;

namespace Splatrika.MobArenaMobile.UI
{
    public class AnimatedNotificationView : NotificationView
    {
        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private string _stateName;

        [SerializeField]
        private TextMeshProUGUI _view;


        public override void Show(string message)
        {
            _view.text = message;
            _animator.Play(_stateName);
        }
    }
}
