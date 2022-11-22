using System;
using System.Collections.Generic;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    public abstract class MobService<TModelConfiguration, TModel, TModelPool,
        TMonoSettings> : IEnemy, IDisposable
        where TModelPool : PoolService<TModelConfiguration, TModel>
        where TModel : IReusable<TModelConfiguration>
        where TMonoSettings : MonoBehaviour, IReusablePresenterProvider
    {
        public int RewardPoints => 0;

        protected TModelPool ModelPool => GetModelPool();

        private Dictionary<int, TMonoSettings[]> _presenterPool;
        private readonly ILogger _logger;
        private TModelPool _modelPool;

        public event Action<IEnemy> Died;

        protected abstract TModelPool CreateModelPool(int poolSize);


        public MobService(
            MobServiceConfiguration configuration,
            ILogger logger)
        {
            _presenterPool = new Dictionary<int, TMonoSettings[]>();
            _logger = logger;

            foreach (var kind in configuration.MobKinds)
            {
                if (!configuration.PresenterPoolsSizes.ContainsKey(kind.KindId))
                {
                    throw new NullReferenceException("Presenter pool size" +
                        $"for kind {kind.KindId} is not specified");
                }
                var size = configuration.PresenterPoolsSizes[kind.KindId];
                _presenterPool.Add(kind.KindId,
                    new TMonoSettings[size]);

                var prefab = Resources.Load<TMonoSettings>(
                    kind.Prefab);
                if (!prefab)
                {
                    throw new NullReferenceException("Failed to load prefab");
                }
                for (var i = 0; i < size; i++)
                {
                    _presenterPool[kind.KindId][i] =
                        GameObject.Instantiate(prefab);
                }
            }
        }


        public void Dispose()
        {
            foreach (var mob in _modelPool.Objects)
            {
                if (mob is IEnemy enemy)
                {
                    enemy.Died -= OnEnemyDied;
                }
            }
        }


        protected TModelPool GetModelPool()
        {
            if (_modelPool == null)
            {
                throw new InvalidOperationException(
                    "AssingModelPool wasn't called in the constructor");
            }
            return _modelPool;
        }


        protected void AssignModelPool(MobServiceConfiguration configuration)
        {
            _modelPool = CreateModelPool(configuration.ModelPoolSize);
            AssignEnemyEvents();
        }


        protected TMonoSettings GetMonoSettings(int kindId)
        {
            if (!_presenterPool.ContainsKey(kindId))
            {
                _logger.LogError("MobService",
                    $"There is no presenter for mobKind {kindId}");
                return null;
            }
            var presenterPool = _presenterPool[kindId];
            TMonoSettings monoSettings = null;
            for (var i = 0; i < presenterPool.Length; i++)
            {
                if (!presenterPool[i].Presenter.Active)
                {
                    monoSettings = presenterPool[i];
                    break;
                }
            }
            if (monoSettings == null)
            {
                _logger.LogError(nameof(ShootingMobService),
                    $"There is no free presenters in the pool");
                return null;
            }
            return monoSettings;
        }


        private void AssignEnemyEvents()
        {
            foreach (var mob in _modelPool.Objects)
            {
                if (mob is IEnemy enemy)
                {
                    enemy.Died += OnEnemyDied;
                }
            }
        }


        private void OnEnemyDied(IEnemy enemy)
        {
            Died?.Invoke(enemy);
        }
    }
}
