using System;

namespace Splatrika.MobArenaMobile.Model
{
    public class RegeneratingAction : IUpdatable
    {
        public bool Active { get; private set; }
        public bool Ready => _time >= RegenerationTime;
        public float TimeLeft => RegenerationTime - _time;
        public float RegenerationTime { get; private set; }

        private float _time;
        private readonly Action _action;
        private readonly ITimeScaleService _timeScaleService;


        public RegeneratingAction(
            Action action,
            ITimeScaleService timeScaleService = null,
            float regenerationTime = 1)
        {
            _action = action;
            RegenerationTime = regenerationTime;
            _time = RegenerationTime;
            _timeScaleService = timeScaleService;
        }


        public void Reset(float regenerationTime)
        {
            _time = regenerationTime;
            RegenerationTime = regenerationTime;
            Stop();
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
            _time += deltaTime * _timeScaleService.TimeScale;
            if (!Active && _time > RegenerationTime)
            {
                _time = RegenerationTime;
            }
            if (Active && _time >= RegenerationTime)
            {
                while (_time >= RegenerationTime)
                {
                    _time -= RegenerationTime;
                    _action.Invoke();
                }
            }
        }
    }
}
