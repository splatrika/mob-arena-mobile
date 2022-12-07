using System;
using System.Collections.Generic;
using Splatrika.MobArenaMobile.Model;
using Zenject;

namespace Splatrika.MobArenaMobile.Factories
{
    public class ModelPool<T> : IModelPool<T>, IDisposable
        where T : IActiveStatus
    {
        private List<T> _all;
        private Queue<T> _pool;


        public ModelPool(
            ModelPoolConfiguration configuration,
            DiContainer container)
        {
            _all = new List<T>(configuration.PoolSize);
            _pool = new Queue<T>(configuration.PoolSize);
            for (int i = 0; i < configuration.PoolSize; i++)
            {
                var model = container.Instantiate<T>();
                _all.Add(model);
                _pool.Enqueue(model);
            }
        }


        public void Dispose()
        {
            foreach (var model in _all)
            {
                if (model is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }


        public T Take()
        {
            if (!_pool.TryDequeue(out T instance))
            {
                throw new InvalidOperationException("There is no free items");
            }
            return instance;
        }


        public void Return(T taken)
        {
            if (taken.Active)
            {
                throw new InvalidOperationException(
                    "Unable to return active instance");
            }
            _pool.Enqueue(taken);
        }
    }
}
