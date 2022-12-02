using Splatrika.MobArenaMobile.UI;
using Zenject;

namespace Splatrika.MobArenaMobile.Startup
{
    public class StartScreenInstaller : MonoInstaller
    {
        public override void Start()
        {
            base.Start();
            var canvas = Container.CreateDefaultCanvas();
            Container.InstantiateUI<StartScreenUI>(
                resourceName: "StartScreenUI",
                canvas: canvas);
        }
    }
}
