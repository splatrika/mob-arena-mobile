using System;
using Splatrika.MobArenaMobile.Model;
using Splatrika.MobArenaMobile.Settings;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Factories
{
    public class WalkingMobService : MobService<WalkingMobMonoSettings,
        WalkingMob, WalkingMobConfiguration, WalkingMobSpawnArgs>,
        IWalkingMobService
    {
        public WalkingMobService(
            SpawnObjectServiceConfiguration<WalkingMobMonoSettings> configuration,
            DiContainer container)
            : base(configuration, container)
        {
        }


        public IDamageable Spawn(int kindId, Vector3 position)
        {
            return Spawn(kindId, new WalkingMobSpawnArgs(position));
        }


        protected override WalkingMobConfiguration BindConfiguration(
            WalkingMobMonoSettings presenter, WalkingMobSpawnArgs spawnArgs)
        {
            return new WalkingMobConfiguration(
                health: presenter.Health,
                position: spawnArgs.Position,
                rewardPoints: presenter.RewardPoints,
                walkingRegenerationTime: presenter.WalkingRegenerationTime,
                speed: presenter.Speed,
                attackDistance: presenter.AttackDistance,
                damage: presenter.AttackDamage,
                attackRegenerationTime: presenter.AttackRegenerationTime);
        }


        protected override void InitPrefab(
            WalkingMob model, WalkingMobMonoSettings presenter)
        {
            presenter.Presenter.Init(model);
        }
    }
}
