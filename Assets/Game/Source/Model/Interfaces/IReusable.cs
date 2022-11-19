using System;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IReusable<TConfiguration>: IActiveStatus
    {
        event Action Activated;
        event Action Deactivated;

        void Start(TConfiguration configuration);
    }
}
