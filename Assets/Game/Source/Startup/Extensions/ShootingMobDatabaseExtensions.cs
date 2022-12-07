using Splatrika.MobArenaMobile.Settings;
using Zenject;

namespace Splatrika.MobArenaMobile.Startup
{
    public static class ShootingMobDatabaseExtensions
    {
        public static void BindShootingMobDatabase(this DiContainer container)
        {
            container.BindSettingsFromResource<ShootingMobDatabaseSettings>
                ("ShootingMobDatabase");
        }
    }
}
