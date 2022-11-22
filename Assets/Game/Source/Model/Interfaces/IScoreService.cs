using System;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IScoreService
    {
        int Score { get; }

        event Action<int> Updated;
    }
}
