using Splatrika.MobArenaMobile.Presenter;
using Zenject;

namespace Splatrika.MobArenaMobile.Scene
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
