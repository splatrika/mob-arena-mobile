using Splatrika.MobArenaMobile.Settings;
using Zenject;

namespace Splatrika.MobArenaMobile.Startup
{
    public static class PlayerSettingsExtensions
    {
        public static void BindPlayerSettings(this DiContainer container)
        {
            container.BindSettingsFromResource<PlayerSettings>
                ("PlayerSettings");
        }
    }
}
