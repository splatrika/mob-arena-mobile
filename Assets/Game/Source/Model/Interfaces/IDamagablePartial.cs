using System;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IDamagablePartial: IHealth, IDamagable
    {
        void Setup(DamagableConfiguration configuration);
    }
}
