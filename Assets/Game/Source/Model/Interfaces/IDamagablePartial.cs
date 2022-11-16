using System;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IDamagablePartial: IHealth, IDamagable
    {
        event Action Damaged;

        void Setup(DamagableConfiguration configuration);
    }
}
