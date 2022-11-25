using Splatrika.MobArenaMobile.Presenter;
using Zenject;

namespace Splatrika.MobArenaMobile.Scene
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
