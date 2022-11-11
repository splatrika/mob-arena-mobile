using System;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IHealth
    {
        event Action Died;
        event Action<int> HealthUpdated;

        bool IsDied { get; }
        int Health { get; }
    }
}
