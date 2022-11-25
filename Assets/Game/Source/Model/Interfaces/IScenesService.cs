using System;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IScenesService
    {
        event Action LoadingStarted;
        event Action LoadingFinished;

        void Load(string scene);
    }
}
