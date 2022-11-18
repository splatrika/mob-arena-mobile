using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public struct LevelConfiguration
    {
        public WaveConfiguration[] Waves;
        public float TimeScaleAcceleration;

        public LevelConfiguration(
            WaveConfiguration[] waves,
            float timeScaleAcceleration)
        {
            Waves = waves;
            TimeScaleAcceleration = timeScaleAcceleration;
        }
    }
}
