using Splatrika.MobArenaMobile.Settings;
using Zenject;

namespace Splatrika.MobArenaMobile.Factories
{
    public class WalkingMobServiceFactory : MobServiceFactory<
        WalkingMobService, WalkingMobMonoSettings,
        WalkingMobSpawnPointMonoSettngs>
    {
        public WalkingMobServiceFactory(
            LevelMonoSettings levelMonoSettings,
            WalkingMobDatabaseSettings kindsDatabase,
            DiContainer container)
            : base(levelMonoSettings, kindsDatabase, container)
        {
        }

        protected override WalkingMobService CreateService(
            SpawnObjectServiceConfiguration<WalkingMobMonoSettings> configuration)
        {
            return new WalkingMobService(configuration, Container);
        }
    }
}
