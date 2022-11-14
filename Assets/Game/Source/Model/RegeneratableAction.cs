using System;

namespace Splatrika.MobArenaMobile.Model
{
    public class RegeneratableAction : IUpdatable
    {
        public bool Active { get; private set; }
        public bool Ready => _time >= RegenerationTime;
        public float TimeLeft => RegenerationTime - _time;
        public float RegenerationTime { get; private set; }
        
        private float _time;
        private readonly Action _action;
        private readonly ITimeScaleService _timeScaleService;


        public RegeneratableAction(
            float regenerationTime,
            Action action,
            ITimeScaleService timeScaleService = null)
        {
            _action = action;
            RegenerationTime = regenerationTime;
            _time = RegenerationTime;
            _timeScaleService = timeScaleService;
        }


        public void Reset(float regenerationTime)
        {
            _time = 0;
            RegenerationTime = regenerationTime;
        }


        public void Start()
        {
            Active = true;
        }


        public void Stop()
        {
            Active = false;
        }


        public void Update(float deltaTime)
        {
            if (_time < RegenerationTime)
            {
                _time += deltaTime * _timeScaleService.TimeScale;
            }
            if (Active)
            {
                _time = 0;
                _action.Invoke();
            }
        }
    }
}
