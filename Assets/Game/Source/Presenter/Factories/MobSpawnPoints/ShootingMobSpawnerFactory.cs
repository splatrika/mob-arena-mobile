using Splatrika.MobArenaMobile.Model;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class ShootingMobSpawnerFactory : IMobSpawnPointFactory
    {
        private readonly IShootingMobService _shootingMobService;


        public ShootingMobSpawnerFactory(IShootingMobService shootingMobService)
        {
            _shootingMobService = shootingMobService;
        }


        public bool CanCreateFrom(MobSpawnPointMonoSettings settings)
        {
            return settings is ShootingMobSpawnerMonoSettings;
        }


        public MobSpawnPoint Create(MobSpawnPointMonoSettings settings)
        {
            var concreteSettings = settings as ShootingMobSpawnerMonoSettings;

            var configuration = new ShootingMobSpawnerConfiguration(
                spawnTime: concreteSettings.SpawnTime,
                kindId: concreteSettings.MobKind,
                spawnPosition: concreteSettings.transform.position,
                shootingPosition: concreteSettings.ShootingPosition.position);

            return new ShootingMobSpawner(configuration, _shootingMobService);
        }
    }
}
