using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public abstract class PoolService<TConfiguration, TModel>
        where TModel : IReusable<TConfiguration>
    {
        public IReadOnlyList<TModel> Objects;

        private List<TModel> _objects;
        private readonly ILogger _logger;

        public delegate void IndexAction(int index);

        public event IndexAction Activated;
        public event IndexAction Deactivated;

        protected abstract TModel CreateInstance();


        public PoolService(int poolSize, ILogger logger)
        {
            _objects = new List<TModel>(poolSize);
            for (var i = 0; i < poolSize; i++)
            {
                _objects[i] = CreateInstance();
                _objects[i].Activated += () => Activated?.Invoke(i);
                _objects[i].Deactivated += () => Deactivated?.Invoke(i);
            }
            Objects = _objects.AsReadOnly();

            _logger = logger;
        }


        public void Spawn(TConfiguration configuration)
        {
            var free = _objects.FirstOrDefault(x => !x.Active);
            if (free == null)
            {
                _logger.LogError(nameof(PoolService<TConfiguration, TModel>),
                    "Object wasn't spawned because there is no free object" +
                    "in the pool");
            }
            free.Start(configuration);
        }
    }
}
