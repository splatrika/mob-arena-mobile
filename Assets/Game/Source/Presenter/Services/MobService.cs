using System;
using System.Collections.Generic;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    public abstract class MobService<TModelConfiguration, TModel, TModelPool,
        TMonoSettings>
        where TModelPool : PoolService<TModelConfiguration, TModel>
        where TModel : IReusable<TModelConfiguration>
        where TMonoSettings : MonoBehaviour, IReusablePresenterProvider
    {
        protected TModelPool ModelPool { get; private set; }

        private Dictionary<int, TMonoSettings[]> _presenterPool;
        private readonly ILogger _logger;

        protected abstract TModelPool CreateModelPool(int poolSize);


        public MobService(
            MobServiceConfiguration configuration)
        {
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

            ModelPool = CreateModelPool(configuration.ModelPoolSize);
        }


        protected TMonoSettings GetMonoSettings(int kindId)
        {
            var presenterPool = _presenterPool[kindId];
            TMonoSettings monoSettings = null;
            for (var i = 0; i < presenterPool.Length; i++)
            {
                if (presenterPool[i].Presenter.Active)
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
    }
}
