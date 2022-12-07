using Splatrika.MobArenaMobile.Model;
using Splatrika.MobArenaMobile.Settings;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Factories
{
    public class ShootingMobService : SpawnObjectService<ShootingMobMonoSettings,
        ShootingMob, ShootingMobConfiguration, ShootingMobSpawnArgs>,
        IShootingMobService
    {
        public ShootingMobService(
            SpawnObjectServiceConfiguration<ShootingMobMonoSettings> configuration,
            DiContainer container)
            : base(configuration, container)
        {
        }


        public IDamageable Spawn(int kindId, Vector3 position,
            Vector3 shootingPosition)
        {
            return Spawn(kindId,
                new ShootingMobSpawnArgs(position, shootingPosition));
        }


        protected override ShootingMobConfiguration BindConfiguration(
            ShootingMobMonoSettings presenter, ShootingMobSpawnArgs spawnArgs)
        {
            return new ShootingMobConfiguration(
                health: presenter.Health,
                position: spawnArgs.Position,
                rewardPoints: presenter.RewardPoints,
                walkingRegenerationTime: presenter.MovementRegenerationTime,
                speed: presenter.MovementSpeed,
                bulletsSpeed: presenter.BulletsSpeed,
                bulletsDamage: presenter.BulletsDamage,
                gunRegenerationTime: presenter.GunRegenerationTime,
                shotPoint: presenter.ShotPoint,
                shootingPosition: spawnArgs.ShootingPosition);
        }


        protected override void InitPrefab(ShootingMob model,
            ShootingMobMonoSettings presenter)
        {
            presenter.Presenter.Init(model);
        }
    }
}
