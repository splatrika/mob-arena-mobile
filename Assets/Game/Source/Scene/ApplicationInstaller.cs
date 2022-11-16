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

            Container.BindPlayerSettings();

            Container.BindNavigationSettings();
        }
    }
}
