using System;

namespace Splatrika.MobArenaMobile.Model
{
    public class Level : ILevel, IUpdatable, IDisposable
    {
        public int WavesCount => _waves.Length;
        public int CurrentWave { get; private set; }
        public int Lap { get; private set; }
        public int FinishedWays => Lap * WavesCount + CurrentWave;
        public bool IsFinished { get; private set; }

        private readonly float _timeScaleAcceleration;
        private readonly Wave[] _waves;
        private readonly IPlayerCharacter _playerCharacter;
        private readonly ITimeScaleService _timeScaleService;

        public event Action Finished;
        public event Action<int> WaveChanged;


        public Level(
            LevelConfiguration configuration,
            IPlayerCharacter playerCharacter,
            ITimeScaleService timeScaleService)
        {
            _playerCharacter = playerCharacter;
            _timeScaleService = timeScaleService;

            _playerCharacter.Died += OnPlayerCharacterDied;

            _waves = new Wave[configuration.Waves.Length];
            for (int i = 0; i < _waves.Length; i++)
            {
                _waves[i] = new Wave(
                    configuration: configuration.Waves[i],
                    timeScaleService: timeScaleService);
                _waves[i].Finished += OnWaveFinished;
            }

            if (_waves.Length > 0)
            {
                _waves[0].Start();
            }

            _timeScaleAcceleration = configuration.TimeScaleAcceleration;
        }


        public void Dispose()
        {
            _playerCharacter.Died -= OnPlayerCharacterDied;
        }


        public void Update(float deltaTime)
        {
            if (!IsFinished)
            {
                _waves[CurrentWave].Update(deltaTime);
            }
        }


        private void OnWaveFinished()
        {
            CurrentWave++;
            if (CurrentWave == _waves.Length)
            {
                _timeScaleService.Up(_timeScaleAcceleration);
                CurrentWave = 0;
                Lap++;
            }
            _waves[CurrentWave].Start();
            WaveChanged?.Invoke(CurrentWave);
        }


        private void OnPlayerCharacterDied()
        {
            IsFinished = true;
            Finished?.Invoke();
        }

    }
}
