using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.UI
{
    public class LevelUI : MonoBehaviour
    {
        [SerializeField]
        private NumberView _healthView;

        [SerializeField]
        private NumberView _waveView;

        [SerializeField]
        private JoystickControl _playerControl;

        private IPlayerCharacter _playerCharacter;
        private ILevel _level;
        private ILogger _logger;


        [Inject]
        public void Init(
            IPlayerCharacter playerCharacter,
            ILevel level,
            ILogger logger)
        {
            _playerCharacter = playerCharacter;
            _level = level;
            _logger = logger;

            _playerCharacter.HealthUpdated += OnPlayerHealthUpdated;
            _level.WaveChanged += OnLevelWaveChanged;

            OnPlayerHealthUpdated(_playerCharacter.Health);
            OnLevelWaveChanged(_level.CurrentWave);

            if (_playerControl)
            {
                _playerControl.Touched += OnJoystickTouched;
                _playerControl.Untouched += OnJoystickUntouched;
                _playerControl.OffsetUpdated += OnJoystickOffsetUpdated;
            }
            else
            {
                _logger.LogWarning(nameof(LevelUI),
                    "Player control is not assigned");
            }
        }


        private void OnDestroy()
        {
            _playerCharacter.HealthUpdated -= OnPlayerHealthUpdated;
            _level.WaveChanged -= OnLevelWaveChanged;

            _playerControl.Touched -= OnJoystickTouched;
            _playerControl.Untouched -= OnJoystickUntouched;
            _playerControl.OffsetUpdated -= OnJoystickOffsetUpdated;
        }


        private void OnPlayerHealthUpdated(int health)
        {
            if (!_healthView)
            {
                _logger.LogWarning(nameof(LevelUI),
                    "Health view is not assigned");
                return;
            }
            _healthView.Show(health);
        }


        private void OnLevelWaveChanged(int wave)
        {
            if (!_waveView)
            {
                _logger.LogWarning(nameof(LevelUI),
                    "Wave view is not assigned");
                return;
            }
            _waveView.Show(wave);
        }


        private void OnJoystickTouched()
        {
            _playerCharacter.StartShooting();
        }


        private void OnJoystickUntouched()
        {
            _playerCharacter.StopShooting();
        }


        private void OnJoystickOffsetUpdated(Vector2 offset)
        {
            _playerCharacter.SetDirection(offset);
        }
    }
}
