using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public abstract class PoolService<TConfiguration, TModel>
        where TModel : IReusable<TConfiguration>
    {
        public IReadOnlyList<TModel> Objects => GetObjects();

        private int _poolSize;
        private IReadOnlyList<TModel> _readOnlyObjects;
        private List<TModel> _objects;
        private readonly ILogger _logger;

        public delegate void IndexAction(int index);

        public event IndexAction Activated;
        public event IndexAction Deactivated;

        protected abstract TModel CreateInstance();


        public PoolService(int poolSize, ILogger logger)
        {
            _poolSize = poolSize;
            _logger = logger;
        }


        public IReadOnlyList<TModel> GetObjects()
        {
            if (_objects == null)
            {
                throw new InvalidOperationException("CreateObjects method " +
                    "wasn't called in constructor");
            }
            return _readOnlyObjects;
        }


        protected void CreateObjects()
        {
            _objects = new List<TModel>(_poolSize);
            for (var i = 0; i < _poolSize; i++)
            {
                _objects.Add(CreateInstance());
                _objects[i].Activated += () => Activated?.Invoke(i);
                _objects[i].Deactivated += () => Deactivated?.Invoke(i);
            }
            _readOnlyObjects = _objects.AsReadOnly();
        }


        public void Spawn(TConfiguration configuration)
        {
            if (_objects == null)
            {
                throw new InvalidOperationException("CreateObjects method " +
                    "wasn't called in constructor");
            }
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
