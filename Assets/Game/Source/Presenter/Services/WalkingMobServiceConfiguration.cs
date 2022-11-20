using System.Collections.Generic;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class WalkingMobServiceConfiguration : MobServiceConfiguration
    {
        public WalkingMobServiceConfiguration(
            Dictionary<int, int> presenterPoolsSizes,
            MobKindSettings[] mobKinds,
            int modelPoolSize)
            : base(presenterPoolsSizes, mobKinds, modelPoolSize)
        {
        }
    }
}
