using System;
using System.Linq;

namespace Splatrika.MobArenaMobile.Model
{
    public class Wave : IUpdatable, IDisposable
    {
        public float Time { get; private set; }
        public bool Active { get; private set; }

        private MobSpawnPoint[] _points;
        private MobDieHandler[] _mobHalders;
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
            _mobHalders = new MobDieHandler[_points.Length];
            for (int i = 0; i < _mobHalders.Length; i++)
            {
                _mobHalders[i] = new MobDieHandler(diedCallback: OnMobDied);
            }

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
            for (int i = 0; i < _mobHalders.Length; i++)
            {
                _mobHalders[i].Dispose();
            }
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
                    var mob = _points[i].Spawn();
                    RegisterMob(mob);
                    _lastCalledPoint = i;
                }
            }
        }


        private void RegisterMob(IDamageable mob)
        {
            for (int i = 0; i < _mobHalders.Length; i++)
            {
                if (_mobHalders[i].IsFree)
                {
                    _mobHalders[i].Register(mob);
                    return;
                }
            }
        }


        private void Reset()
        {
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
                Active = false;
                Reset();
                Finished?.Invoke();
            }
        }


        private class MobDieHandler : IDisposable
        {
            public bool IsFree => _registered == null;

            private readonly Action _diedCallback;
            private IDamageable _registered;


            public MobDieHandler(
                Action diedCallback)
            {
                _diedCallback = diedCallback;
            }


            public void Dispose()
            {
                if (_registered != null)
                {
                    _registered.Died -= OnDied;
                }
            }


            public void Register(IDamageable health)
            {
                if (!IsFree)
                {
                    throw new InvalidOperationException("Not free");
                }
                _registered = health;
                health.Died += OnDied;
            }


            private void OnDied()
            {
                _diedCallback.Invoke();
                _registered.Died -= OnDied;
                _registered = null;
            }
        }
    }
}
