using System;
using System.Linq;

namespace Splatrika.MobArenaMobile.Model
{
    public class Wave : IUpdatable, IDisposable
    {
        public float Time { get; private set; }
        public bool Active { get; private set; }

        private MobSpawnPoint[] _points;
        private IHealth[] _mobs;
        private int _diedMobs;
        private int _lastCalledPoint;
        private readonly ITimeScaleService _timeScaleService;

        public event Action Finished;


        public Wave(
            WaveConfiguration configuration,
            ITimeScaleService timeScaleService)
        {
            _points = configuration.SpawnPoints
                .OrderBy(x => x.SpawnTime)
                .ToArray();
            _mobs = new IHealth[_points.Length];

            _timeScaleService = timeScaleService;
        }


        public void Start()
        {
            if (Active)
            {
                throw new InvalidOperationException("Already started");
            }
            _lastCalledPoint = -1;
            Reset();
            Active = true;
        }


        public void Dispose()
        {
            Reset();
        }


        public void Update(float deltaTime)
        {
            if (Active)
            {
                Time += deltaTime * _timeScaleService.TimeScale;
                if (_lastCalledPoint == _points.Length)
                {
                    return;
                }
                for (int i = _lastCalledPoint + 1; i < _points.Length; i++)
                {
                    if (Time < _points[i].SpawnTime)
                    {
                        break;
                    }
                    _mobs[i] = _points[i].Spawn();
                    _mobs[i].Died += OnMobDied;
                    _lastCalledPoint = i;
                }
            }
        }


        private void Reset()
        {
            for (int i = 0; i <= _lastCalledPoint; i++)
            {
                _mobs[i].Died -= OnMobDied;
            }
            _lastCalledPoint = -1;
            _diedMobs = 0;
            Active = false;
            Time = 0;
        }


        private void OnMobDied()
        {
            _diedMobs++;
            if (_diedMobs == _points.Length)
            {
                Finished?.Invoke();
                Reset();
            }
        }
    }
}
