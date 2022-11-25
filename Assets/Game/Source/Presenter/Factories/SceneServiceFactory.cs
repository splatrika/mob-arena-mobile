using Splatrika.MobArenaMobile.UI;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class ScenesServiceFactory : IFactory<ScenesService>
    {
        private readonly ILogger _logger;
        private readonly LoadingSettings _loadingSettings;


        public ScenesServiceFactory(
            ILogger logger,
            LoadingSettings loadingSettings)
        {
            _logger = logger;
            _loadingSettings = loadingSettings;
        }


        public ScenesService Create()
        {
            var service = new GameObject("ScenesService")
                .AddComponent<ScenesService>();

            var loadDelay = 0f;
            var uiPrefab = Resources.Load<LoadingUI>(
                _loadingSettings.LoadingUIPrefab);
            if (uiPrefab)
            {
                loadDelay = uiPrefab.GetShowDuration();
            }
            else
            {
                _logger.LogWarning(nameof(ScenesServiceFactory),
                    "Unable to load ui");
            }

            var configuration = new ScenesServiceConfiguration(
                loadDelay);
            service.Init(configuration, _logger);
            GameObject.DontDestroyOnLoad(service);
            return service;
        }
    }
}
