using System;
using System.Collections.Generic;
using System.Linq;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Factories
{
    public abstract class
        SpawnObjectService<TPresenter, TModel, TConfiguration, TSpawnArgs>
        : IDisposable
        where TModel : IReusable<TConfiguration>
        where TPresenter : MonoBehaviour
    {
        private readonly PresenterPool<TPresenter> _presenterPool;
        private readonly ModelPool<TModel> _modelPool;
        private readonly List<Handler> _handlers;

        protected abstract TConfiguration BindConfiguration(
            TPresenter presenter, TSpawnArgs spawnArgs);
        protected abstract void InitPrefab(TModel model, TPresenter presenter);


        public SpawnObjectService(
            SpawnObjectServiceConfiguration<TPresenter> configuration,
            DiContainer container)
        {
            _presenterPool = new PresenterPool<TPresenter>(configuration.Kinds);
            _modelPool = new ModelPool<TModel>(configuration.ModelPool, container);

            _handlers = new List<Handler>(configuration.ModelPool.PoolSize);
            for (int i = 0; i < configuration.ModelPool.PoolSize; i++)
            {
                _handlers.Add(new Handler(_presenterPool, _modelPool));
            }
        }
        

        public TModel Spawn(int kindId, TSpawnArgs spawnArgs)
        {
            var presenter = _presenterPool.Take(kindId);
            var configuration = BindConfiguration(presenter, spawnArgs);
            var model = _modelPool.Take();

            var handler = _handlers.FirstOrDefault(x => !x.Busy);
            if (handler == null)
            {
                throw new NullReferenceException(
                    "There is no any free handler");
            }
            handler.Register(model, presenter, kindId);

            model.Start(configuration);
            InitPrefab(model, presenter);

            return model;
        }


        public void Dispose()
        {
            _modelPool.Dispose();
            _presenterPool.Dispose();
        }


        private class Handler : IDisposable
        {
            public bool Busy { get; private set; }

            private int _kindId;
            private TModel _model;
            private TPresenter _presenter;
            private readonly IPresenterPool<TPresenter> _presenterPool;
            private readonly IModelPool<TModel> _modelPool;


            public Handler(
                IPresenterPool<TPresenter> presenterPool,
                IModelPool<TModel> modelPool)
            {
                _presenterPool = presenterPool;
                _modelPool = modelPool;
            }


            public void Register(TModel model, TPresenter presenter, int kindId)
            {
                if (Busy)
                {
                    throw new InvalidOperationException("Handler is busy");
                }
                _model = model;
                _presenter = presenter;
                _kindId = kindId;
                _model.Deactivated += OnDeactivated;
                Busy = true;
            }


            private void OnDeactivated()
            {
                _presenterPool.Return(_kindId, _presenter);
                _modelPool.Return(_model);
                _model = default;
                _presenter = default;
                _model.Deactivated -= OnDeactivated;
                Busy = false;
            }


            public void Dispose()
            {
                if (Busy)
                {
                    OnDeactivated();
                }
            }
        }
    }
}
