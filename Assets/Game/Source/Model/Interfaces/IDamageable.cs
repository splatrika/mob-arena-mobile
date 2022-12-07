using System;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IDamageable
    {
        bool IsDied { get; }
        int Health { get; }

        event Action Damaged;
        event Action Died;
        event Action<int> HealthUpdated;

        void Damage(IDamager damager);
    }
}
