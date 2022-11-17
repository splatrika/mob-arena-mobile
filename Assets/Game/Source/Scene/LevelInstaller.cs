using Splatrika.MobArenaMobile.Model;
using Splatrika.MobArenaMobile.Presenter;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Scene
{
    public class LevelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerCharacter>()
                .FromFactory<PlayerCharacter, PlayerAtSpawnPointFactory>()
                .AsSingle()
                .NonLazy();

            Container.Bind<ITimeScaleService>()
                .To<StaticTimeScaleService>()
                .AsSingle();

            Container.Bind<IFriendBulletService>()
                .To<FakeFriendBulletService>()
                .AsSingle();

            Container.Bind<INavigationService>()
                .To<NavigationAdapter>()
                .AsSingle();

            Container.Bind<IBulletService>()
                .To<FakeBulletService>()
                .AsSingle();

            Container.Bind<IRaycastService>()
                .To<RaycastAdapter>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<WalkingMob>()
                .FromFactory<WalkingMob, WalkingMobFromSceneFactory>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesAndSelfTo<ShootingMob>()
                .FromFactory<ShootingMob, ShootingMobFromSceneFactory>()
                .AsSingle()
                .NonLazy();
        }


        public override void Start()
        {
            base.Start();

            Container.InstantiateComponent<Updater>(new GameObject());
        }
    }
}
