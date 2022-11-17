using Splatrika.MobArenaMobile.Presenter;
using Zenject;

namespace Splatrika.MobArenaMobile.Scene
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
