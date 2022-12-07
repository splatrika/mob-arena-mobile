using System;
using System.Collections.Generic;

namespace Splatrika.MobArenaMobile.Model
{
    public class ScoreService : IScoreService, IDisposable
    {
        public int Score { get; private set; }

        private readonly List<IRewardProvider> _providers;

        public event Action<int> Updated;


        public ScoreService(List<IRewardProvider> providers)
        {
            _providers = providers;
            foreach (var provider in providers)
            {
                provider.Rewarded += OnRewarded;
            }
        }


        public void Dispose()
        {
            foreach (var provider in _providers)
            {
                provider.Rewarded -= OnRewarded;
            }
        }


        private void OnRewarded(int points)
        {
            Score += points;
            Updated?.Invoke(Score);
        }
    }
}
