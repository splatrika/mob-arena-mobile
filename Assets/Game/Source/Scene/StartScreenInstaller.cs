using Splatrika.MobArenaMobile.UI;
using Zenject;

namespace Splatrika.MobArenaMobile.Scene
{
    public class StartScreenInstaller : MonoInstaller
    {
        public override void Start()
        {
            base.Start();
            Container.InstantiateUI<StartScreenUI>(
                resourceName: "StartScreenUI");
        }
    }
}
