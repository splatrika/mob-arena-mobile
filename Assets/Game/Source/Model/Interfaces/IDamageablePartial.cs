using System;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IDamageablePartial: IHealth, IDamageable
    {
        void Setup(DamageableConfiguration configuration);
    }
}
