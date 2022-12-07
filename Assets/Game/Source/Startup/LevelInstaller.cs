using Splatrika.MobArenaMobile.Factories;
using Splatrika.MobArenaMobile.Model;
using Splatrika.MobArenaMobile.Presenter;
using Splatrika.MobArenaMobile.Settings;
using Splatrika.MobArenaMobile.UI;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Startup
{
    public class LevelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LevelMonoSettings>()
                .To<LevelMonoSettings>()
                .FromFactory<LevelMonoSettingsFactory>()
                .AsSingle();

            Container.Bind<ITimeScaleService>()
                .To<TimeScaleService>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<Level>()
                .FromFactory<Level, LevelFactory>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesTo<LevelEnvironment>()
                .FromFactory<LevelEnvironment, LevelEnvironmentFactory>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesAndSelfTo<FriendBulletService>()
                .FromFactory<FriendBulletService, FriendBulletServiceFactory>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesAndSelfTo<PlayerCharacter>()
                .FromFactory<PlayerCharacter, PlayerAtSpawnPointFactory>()
                .AsSingle()
                .NonLazy();

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

            Container.BindInterfacesTo<ScoreService>()
                .AsSingle()
                .NonLazy();

            Container.Bind<NavigationConfiguration>()
                .FromFactory<NavigationConfigurationFactory>()
                .AsSingle();
        }


        public override void Start()
        {
            base.Start();

            Container.InstantiateComponent<Updater>(new GameObject());

            var canvas = Container.CreateDefaultCanvas();
            Container.InstantiateUI<LevelUI>(
                resourceName: "LevelUI",
                canvas: canvas);
            Container.InstantiateUI<WaveNotificationUI>(
                resourceName: "WaveNotificationUI",
                canvas: canvas);
            Container.InstantiateUI<GameOverUI>(
                resourceName: "GameOverUI",
                canvas: canvas);
        }
    }
}
