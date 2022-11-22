using System;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IEnemy
    {
        int RewardPoints { get; }

        event Action<IEnemy> Died;
    }
}
