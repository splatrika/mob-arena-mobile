using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class TimeScaleService : ITimeScaleService
    {
        public float TimeScale { get; private set; }

        private readonly ILogger _logger;


        public TimeScaleService(ILogger logger)
        {
            TimeScale = 1;
            _logger = logger;
        }


        public void Up(float value)
        {
            if (value < 0)
            {
                _logger.LogError(nameof(TimeScaleService),
                    "Value cannot be lesse than 0");
                return;
            }
            _logger.Log($"New timescale: {TimeScale}");
            TimeScale += value;
        }
    }
}
