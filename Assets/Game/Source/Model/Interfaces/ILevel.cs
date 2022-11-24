using System;

namespace Splatrika.MobArenaMobile.Model
{
    public interface ILevel
    {
        event Action Finished;
        event Action<int> WaveChanged;

        int CurrentWave { get; }
        int Lap { get; }
        int FinishedWays { get; }
    }
}
