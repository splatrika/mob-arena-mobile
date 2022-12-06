using System.Collections.Generic;

namespace Splatrika.MobArenaMobile.Factories
{
    public class SpawnObjectServiceConfiguration<T>
    {
        public List<KindConfiguration<T>> Kinds;
        public ModelPoolConfiguration ModelPool;


        public SpawnObjectServiceConfiguration(
            List<KindConfiguration<T>> kinds,
            ModelPoolConfiguration modelPool)
        {
            Kinds = kinds;
            ModelPool = modelPool;
        }
    }
}
