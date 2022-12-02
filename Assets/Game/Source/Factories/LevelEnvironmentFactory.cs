using Splatrika.MobArenaMobile.Model;
using Splatrika.MobArenaMobile.Presenter;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Factories
{
    public class LevelEnvironmentFactory : IFactory<LevelEnvironment>
    {
        private readonly ILogger _logger;


        public LevelEnvironmentFactory(ILogger logger)
        {
            _logger = logger;
        }


        public LevelEnvironment Create()
        {
            var presenter =
                GameObject.FindObjectOfType<LevelEnvironmentPresenter>();
            if (!presenter)
            {
                _logger.LogWarning(nameof(LevelEnvironmentFactory),
                    "LevelEnviromentPresenter was not found");
            }

            var environment = new LevelEnvironment();
            if (presenter)
            {
                presenter.Init(environment);
            }
            return environment;
        }
    }
}
