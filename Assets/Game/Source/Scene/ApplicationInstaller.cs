using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Scene
{
    public class ApplicationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ILogger>()
                .FromInstance(Debug.unityLogger);

            Container.Bind<IScenesService>()
                .To<FakeScenesService>()
                .AsSingle();

            Container.BindPlayerSettings();

            Container.BindNavigationSettings();

            Container.BindBulletsSettings();

            Container.BindShootingMobDatabase();

            Container.BindWalkingMobDatabase();
        }
    }
}
