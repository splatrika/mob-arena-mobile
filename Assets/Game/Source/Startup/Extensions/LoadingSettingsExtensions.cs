using Splatrika.MobArenaMobile.Settings;
using Zenject;

namespace Splatrika.MobArenaMobile.Startup
{
    public static class LoadingSettingsExtensions
    {
        public static void BindLoadingSettings(this DiContainer container)
        {
            container.BindSettingsFromResource<LoadingSettings>
                ("LoadingSettings");
        }
    }
}
