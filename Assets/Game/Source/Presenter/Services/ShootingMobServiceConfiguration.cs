using System.Collections.Generic;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class ShootingMobServiceConfiguration : MobServiceConfiguration
    {
        public ShootingMobServiceConfiguration(
            Dictionary<int, int> presenterPoolsSizes,
            MobKindSettings[] mobKinds,
            int modelPoolSize)
            : base(presenterPoolsSizes, mobKinds, modelPoolSize)
        {
        }
    }
}
