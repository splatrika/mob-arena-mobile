using Splatrika.MobArenaMobile.Presenter;
using Zenject;

namespace Splatrika.MobArenaMobile.Scene
{
    public static class WalkingMobDatabaseExtensions
    {
        public static void BindWalkingMobDatabase(this DiContainer container)
        {
            container.BindSettingsFromResource<WalkingMobDatabaseSettings>
                ("WalkingMobDatabase");
        }
    }
}
