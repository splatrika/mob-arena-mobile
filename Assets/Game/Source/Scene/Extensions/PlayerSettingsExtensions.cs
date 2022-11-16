using Splatrika.MobArenaMobile.Presenter;
using Zenject;

namespace Splatrika.MobArenaMobile.Scene
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
