using System.Collections.Generic;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class MobServiceConfiguration : MonoBehaviour
    {
        // key: kindId value: size;
        public Dictionary<int, int> PresenterPoolsSizes;
        public MobKindSettings[] MobKinds;
        public int ModelPoolSize;

        public MobServiceConfiguration(
            Dictionary<int, int> presenterPoolsSizes,
            MobKindSettings[] mobKinds,
            int modelPoolSize)
        {
            PresenterPoolsSizes = presenterPoolsSizes;
            MobKinds = mobKinds;
            ModelPoolSize = modelPoolSize;
        }
    }
}
