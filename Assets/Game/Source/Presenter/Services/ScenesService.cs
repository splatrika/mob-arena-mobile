using System;
using System.Collections;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class ScenesService : MonoBehaviour, IScenesService
    {
        private AsyncOperation _currentLoading;
        private float _loadDelay;
        private ILogger _logger;
        private bool _busy;

        public event Action LoadingStarted;
        public event Action LoadingFinished;


        [Inject]
        public void Init(
            ScenesServiceConfiguration configuration,
            ILogger logger)
        {
            _loadDelay = configuration.LoadDelay;
            _logger = logger;
        } 


        public void Load(string scene)
        {
            if (_busy)
            {
                _logger.LogError(nameof(ScenesService),
                    "Service is busy");
            }
            StartCoroutine(LoadingCoroutine(scene));
        }


        private IEnumerator LoadingCoroutine(string scene)
        {
            _busy = true;
            LoadingStarted?.Invoke();
            yield return new WaitForSeconds(_loadDelay);
            _currentLoading = SceneManager.LoadSceneAsync(scene);
            while (!_currentLoading.isDone)
            {
                yield return null;
            }
            GC.Collect();
            _busy = false;
            _currentLoading = null;
            LoadingFinished?.Invoke();
        }
    }
}
