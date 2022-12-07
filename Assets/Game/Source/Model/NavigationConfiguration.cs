using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public struct NavigationConfiguration
    {
        public int PathBuffersSize;


        public NavigationConfiguration(int pathBuffersSize)
        {
            PathBuffersSize = pathBuffersSize;
        }
    }
}
