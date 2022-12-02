using System.Collections.Generic;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Settings
{
    public class LevelMonoSettings : MonoBehaviour
    {
        public IReadOnlyList<WaveMonoSettings> Waves
            => _waves.AsReadOnly();

        public float TimeScaleAcceleration => _timeScaleAcceleration;

        [SerializeField]
        private List<WaveMonoSettings> _waves = new List<WaveMonoSettings>();

        [SerializeField]
        private float _timeScaleAcceleration;


        private void OnValidate()
        {
            if (_timeScaleAcceleration < 0)
            {
                _timeScaleAcceleration = 0;
                Debug.LogError("TimeScaleAcceleration cannot be" +
                    "less than 0");
            }
            foreach (var wave in _waves)
            {
                if (!wave)
                {
                    Debug.LogWarning("There is unassigned wave");
                }
            }
        }
    }
}
