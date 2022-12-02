using Splatrika.MobArenaMobile.Factories;
using Splatrika.MobArenaMobile.Model;
using Splatrika.MobArenaMobile.Settings;
using Splatrika.MobArenaMobile.UI;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Startup
{
    public class ApplicationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ILogger>()
                .FromInstance(Debug.unityLogger);

            Container.Bind<IScenesService>()
                .FromFactory<ScenesServiceFactory>()
                .AsSingle();

            Container.BindPlayerSettings();

            Container.BindNavigationSettings();

            Container.BindBulletsSettings();

            Container.BindShootingMobDatabase();

            Container.BindWalkingMobDatabase();

            Container.BindLoadingSettings();
        }


        public override void Start()
        {
            base.Start();
            Application.targetFrameRate = 60;

            var globalCanvas = Container.CreateDefaultCanvas();
            globalCanvas.sortingOrder = 1000;
            GameObject.DontDestroyOnLoad(globalCanvas);

            var loadingSettings = Container.Resolve<LoadingSettings>();
            Container.InstantiateUI<LoadingUI>(
                resourceName: loadingSettings.LoadingUIPrefab,
                canvas: globalCanvas);
        }
    }
}
