using System;

namespace Splatrika.MobArenaMobile.Model
{
    public class Level : ILevel, ITimeScaleService, IUpdatable, IDisposable
    {
        public int WavesCount => _waves.Length;
        public int CurrentWave { get; private set; }
        public float TimeScale { get; private set; }
        public bool IsFinished { get; private set; }

        private readonly float _timeScaleAcceleration;
        private readonly Wave[] _waves;
        private readonly IPlayerCharacter _playerCharacter;

        public event Action Finished;


        public Level(
            LevelConfiguration configuration,
            IPlayerCharacter playerCharacter)
        {
            _playerCharacter = playerCharacter;

            _playerCharacter.Died += OnPlayerCharacterDied;

            TimeScale = 1;

            var _waves = new Wave[configuration.Waves.Length];
            for (int i = 0; i < _waves.Length; i++)
            {
                _waves[i] = new Wave(
                    configuration: configuration.Waves[i],
                    timeScaleService: this);
                _waves[i].Finished += OnWaveFinished;
            }
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
                TimeScale += _timeScaleAcceleration;
                CurrentWave = 0;
            }
        }


        private void OnPlayerCharacterDied()
        {
            IsFinished = true;
            Finished?.Invoke();
        }

    }
}
