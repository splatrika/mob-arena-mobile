using Splatrika.MobArenaMobile.Settings;
using Zenject;

namespace Splatrika.MobArenaMobile.Factories
{
    public class ShootingMobServiceFactory : MobServiceFactory<
        ShootingMobService, ShootingMobMonoSettings,
        ShootingMobSpawnPointMonoSettings>
    {
        public ShootingMobServiceFactory(
            LevelMonoSettings levelMonoSettings,
            WalkingMobDatabaseSettings kindsDatabase,
            DiContainer container) :
            base(levelMonoSettings, kindsDatabase, container)
        {
        }


        protected override ShootingMobService CreateService(
            SpawnObjectServiceConfiguration<ShootingMobMonoSettings> configuration)
        {
            return new ShootingMobService(configuration, Container);
        }
    }
}
