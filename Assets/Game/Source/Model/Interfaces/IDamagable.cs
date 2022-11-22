using System;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IDamagable
    {
        event Action Damaged;

        void Damage(IDamager damager);
    }
}
