using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Splatrika.MobArenaMobile.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private string _showState;

        [SerializeField]
        private Button _exitButton;

        [SerializeField]
        private NumberView _scoreView;

        // here will be best score view

        private ILevel _level;
        private IScenesService _scenesService;
        private IScoreService _scoreService;
        private ILogger _logger;


        [Inject]
        public void Init(
            ILevel level,
            IScenesService scenesService,
            ILogger logger,
            IScoreService scoreService)
        {
            _level = level;
            _scenesService = scenesService;
            _logger = logger;
            _scoreService = scoreService;

            _level.Finished += OnLevelFinished;

            if (_exitButton)
            {
                _exitButton.onClick.AddListener(OnExitButtonClicked);
            }
            else
            {
                logger.LogWarning(nameof(GameOverUI),
                    "ExitButton is not assigned");
            }

            gameObject.SetActive(false);
        }


        private void Start()
        {
            gameObject.SetActive(false);
        }


        private void OnDestroy()
        {
            _level.Finished -= OnLevelFinished;
        }


        private void OnLevelFinished()
        {
            gameObject.SetActive(true);
            _animator.Play(_showState);
            if (_scoreView)
            {
                _scoreView.Show(_scoreService.Score);
            }
            else
            {
                _logger.LogWarning(nameof(GameOverUI),
                    "ScoreView is not assigned");
            }
        }


        private void OnExitButtonClicked()
        {
            _scenesService.Load(Scenes.StartScreen);
        }
    }
}
