using Splatrika.MobArenaMobile.Settings;
using Zenject;

namespace Splatrika.MobArenaMobile.Startup
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
