using System;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IReusable<TConfiguration>
    {
        bool Active { get; }

        event Action Activated;
        event Action Deactivated;

        void Start(TConfiguration configuration);
    }
}
