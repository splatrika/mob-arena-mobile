using Splatrika.MobArenaMobile.Model;
using Splatrika.MobArenaMobile.Presenter;
using Splatrika.MobArenaMobile.UI;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Scene
{
    public class LevelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LevelMonoSettings>()
                .To<LevelMonoSettings>()
                .FromFactory<LevelMonoSettingsFactory>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<ShootingMobService>()
                .FromFactory<ShootingMobService, ShootingMobServiceFactory>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<WalkingMobService>()
                .FromFactory<WalkingMobService, WalkingMobServiceFactory>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<ShootingMobSpawnerFactory>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<WalkingMobSpawnerFactory>()
                .AsSingle();

            Container.Bind<ITimeScaleService>()
                .To<TimeScaleService>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<Level>()
                .FromFactory<Level, LevelFactory>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesAndSelfTo<PlayerCharacter>()
                .FromFactory<PlayerCharacter, PlayerAtSpawnPointFactory>()
                .AsSingle()
                .NonLazy();

            Container.Bind<IFriendBulletService>()
                .To<FakeFriendBulletService>()
                .AsSingle();

            Container.Bind<INavigationService>()
                .To<NavigationAdapter>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<BulletService>()
                .FromFactory<BulletService, BulletServiceFactory>()
                .AsSingle()
                .NonLazy();

            Container.Bind<IRaycastService>()
                .To<RaycastAdapter>()
                .AsSingle();
        }


        public override void Start()
        {
            base.Start();

            Container.InstantiateComponent<Updater>(new GameObject());
            Container.InstantiateUI<LevelUI>(resourceName: "LevelUI");
        }
    }
}
