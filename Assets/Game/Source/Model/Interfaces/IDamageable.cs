using System;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IDamageable
    {
        event Action Damaged;

        void Damage(IDamager damager);
    }
}
