using Splatrika.MobArenaMobile.Presenter;
using Zenject;

namespace Splatrika.MobArenaMobile.Scene
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
