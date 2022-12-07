using System;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IDamageablePartial: IDamageable
    {
        void Setup(DamageableConfiguration configuration);
    }
}
