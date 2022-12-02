using Splatrika.MobArenaMobile.UI;
using Zenject;

namespace Splatrika.MobArenaMobile.Startup
{
    public class CreditsScreenInstaller : MonoInstaller
    {
        public override void Start()
        {
            base.Start();
            var canvas = Container.CreateDefaultCanvas();
            Container.InstantiateUI<CreditsUI>(
                resourceName: "CreditsUI",
                canvas: canvas);
        }
    }
}
