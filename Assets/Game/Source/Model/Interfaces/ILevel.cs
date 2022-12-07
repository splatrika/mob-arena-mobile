using System;

namespace Splatrika.MobArenaMobile.Model
{
    public interface ILevel
    {
        event Action Finished;
        event Action<int> WaveChanged; // TODO REFACT возможно декомпозировать
        // ибо почему time scale и score отдельно, а это всё внутри.
        // Или наоборот собрать внутрь как описание жизненного цикла уровня

        int CurrentWave { get; }
        int Lap { get; }
        int FinishedWays { get; }
    }
}
