using Splatrika.MobArenaMobile.Model;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class WalkingMobSpawnerFactory : IMobSpawnPointFactory
    {
        private readonly IWalkingMobService _walkingMobService;


        public WalkingMobSpawnerFactory(IWalkingMobService walkingMobService)
        {
            _walkingMobService = walkingMobService;
        }


        public bool CanCreateFrom(MobSpawnPointMonoSettings settings)
        {
            return settings is WalkingMobSpawnerMonoSettings;
        }


        public MobSpawnPoint Create(MobSpawnPointMonoSettings settings)
        {
            var concreteSettings = settings as WalkingMobSpawnerMonoSettings;

            var configuration = new WalkingMobSpawnerConfiguration(
                spawnTime: concreteSettings.SpawnTime,
                kindId: concreteSettings.MobKind,
                position: concreteSettings.transform.position);

            return new WalkingMobSpawner(configuration, _walkingMobService);
        }
    }
}
