using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.UI
{
    public class WaveNotificationUI : MonoBehaviour
    {
        [SerializeField]
        private NotificationView _view;

        [SerializeField]
        private string _messageFormat = "Wave {0}";

        private ILevel _level;


        [Inject]
        public void Init(ILevel level)
        {
            _level = level;

            _level.WaveChanged += OnWaveChanged;
        }


        private void OnDestroy()
        {
            _level.WaveChanged -= OnWaveChanged;
        }


        private void OnWaveChanged(int wave)
        {
            _view.Show(string.Format(_messageFormat, _level.FinishedWays + 1));
        }
    }
}
