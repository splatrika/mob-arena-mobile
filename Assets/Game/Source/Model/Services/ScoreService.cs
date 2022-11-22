using System;
using System.Collections.Generic;

namespace Splatrika.MobArenaMobile.Model
{
    public class ScoreService : IScoreService, IDisposable
    {
        public int Score { get; private set; }

        private readonly List<IEnemy> _enemies;

        public event Action<int> Updated;


        public ScoreService(List<IEnemy> enemies)
        {
            _enemies = enemies;
            foreach (var enemy in enemies)
            {
                enemy.Died += OnEnemyDied;
            }
        }


        public void Dispose()
        {
            foreach (var enemy in _enemies)
            {
                enemy.Died -= OnEnemyDied;
            }
        }


        private void OnEnemyDied(IEnemy enemy)
        {
            Score += enemy.RewardPoints;
            Updated?.Invoke(Score);
        }
    }
}
