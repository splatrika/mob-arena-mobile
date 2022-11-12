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
            Container.Bind<PlayerCharacter>()
                .FromFactory<PlayerAtSpawnPointFactory>()
                .AsSingle()
                .NonLazy();

            Container.Bind<ITimeScaleService>()
                .To<StaticTimeScaleService>()
                .AsSingle();

            Container.Bind<IFriendBulletService>()
                .To<FakeFriendBulletService>()
                .AsSingle();
        }


        public override void Start()
        {
            base.Start();

            Container.InstantiateComponent<Updater>(new GameObject());
        }
    }
}
