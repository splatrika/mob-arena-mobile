using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class LevelMonoSettingsFactory : IFactory<LevelMonoSettings>
    {
        private readonly ILogger _logger;


        public LevelMonoSettingsFactory(ILogger logger)
        {
            _logger = logger;
        }


        public LevelMonoSettings Create()
        {
            var settings = GameObject.FindObjectOfType<LevelMonoSettings>();
            if (!settings)
            {
                _logger.LogError(nameof(LevelMonoSettingsFactory),
                    "There is no LevelMonoSettings on the scene");
            }
            return settings;
        }
    }
}
