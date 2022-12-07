using Splatrika.MobArenaMobile.Model;
using Splatrika.MobArenaMobile.Settings;
using Zenject;

namespace Splatrika.MobArenaMobile.Factories
{
    public class NavigationConfigurationFactory
        : IFactory<NavigationConfiguration>
    {
        private NavigationSettings _settings;


        public NavigationConfigurationFactory(NavigationSettings settings)
        {
            _settings = settings;
        }


        public NavigationConfiguration Create()
        {
            return new NavigationConfiguration(_settings.PathBufferSize);
        }
    }
}
