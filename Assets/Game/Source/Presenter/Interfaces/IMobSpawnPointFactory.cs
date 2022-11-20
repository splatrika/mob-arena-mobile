using Splatrika.MobArenaMobile.Model;

namespace Splatrika.MobArenaMobile.Presenter
{
    public interface IMobSpawnPointFactory
    {
        bool CanCreateFrom(MobSpawnPointMonoSettings settings);

        MobSpawnPoint Create(MobSpawnPointMonoSettings settings);
    }
}
