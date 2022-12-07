using Splatrika.MobArenaMobile.Settings;
using Zenject;

namespace Splatrika.MobArenaMobile.Startup
{
    public static class BulletsSettingsExtension
    {
        public static void BindBulletsSettings(this DiContainer container)
        {
            container.BindSettingsFromResource<BulletsSettings>
                ("BulletsSettings");
        }
    }
}
