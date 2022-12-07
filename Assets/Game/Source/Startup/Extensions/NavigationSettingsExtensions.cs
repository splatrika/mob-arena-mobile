using Splatrika.MobArenaMobile.Settings;
using Zenject;

namespace Splatrika.MobArenaMobile.Startup
{
    public static class NavigationSettingsExtensions
    {
        public static void BindNavigationSettings(this DiContainer container)
        {
            container.BindSettingsFromResource<NavigationSettings>
                ("NavigationSettings");
        }
    }
}
