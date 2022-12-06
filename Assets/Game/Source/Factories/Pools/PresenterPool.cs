using System;
using System.Collections.Generic;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Factories
{
    public class PresenterPool<T> : IPresenterPool<T>, IDisposable
        where T : MonoBehaviour
    {
        private Dictionary<int, KindCollections> _kindPools;


        public PresenterPool(IEnumerable<KindConfiguration<T>> configuration)
        {
            foreach (var kind in configuration)
            {
                var collections = new KindCollections(kind.PoolSize);
                for (int i = 0; i < kind.PoolSize; i++)
                {
                    var instance = GameObject.Instantiate(kind.Prefab);
                    instance.gameObject.SetActive(false);
                    collections.Pool.Enqueue(instance);
                    collections.All.Add(instance);
                }
                _kindPools.Add(kind.KindId, collections);
            }
        }


        public void Dispose()
        {
            foreach (var collections in _kindPools.Values)
            {
                foreach (var presenter in collections.All)
                {
                    GameObject.Destroy(presenter);
                }
            }
        }


        public void Return(int kindId, T taken)
        {
            var pool = GetPool(kindId);
            pool.Enqueue(taken);
            taken.gameObject.SetActive(false);
        }


        public T Take(int kindId)
        {
            var pool = GetPool(kindId);
            if (!pool.TryDequeue(out T instance))
            {
                throw new InvalidOperationException("There is no free items");

            }
            return instance;
        }


        private Queue<T> GetPool(int kindId)
        {
            if (!_kindPools.ContainsKey(kindId))
            {
                throw new InvalidOperationException("Unknown kind");
            }
            var collections = _kindPools[kindId];
            return collections.Pool;
        }


        private class KindCollections
        {
            public Queue<T> Pool { get; private set; }
            public List<T> All { get; private set; }


            public KindCollections(int capacity)
            {
                Pool = new Queue<T>(capacity);
                All = new List<T>(capacity);
            }
        }
    }
}
