using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Factories
{
    public abstract class MobService<TPresenter, TMob, TConfiguration, TSpawnArgs>
        : SpawnObjectService<TPresenter, TMob, TConfiguration, TSpawnArgs>,
        IRewardProvider
        where TConfiguration : MobConfiguration
        where TMob : Mob<TConfiguration>
        where TPresenter : MonoBehaviour
    {
        private List<MobHandler> _mobHandlers;

        public event IRewardProvider.RewardedAction Rewarded;


        protected MobService(
            SpawnObjectServiceConfiguration<TPresenter> configuration,
            DiContainer container)
            : base(configuration, container)
        {
            _mobHandlers = new List<MobHandler>();
            for (int i = 0; i < configuration.ModelPool.PoolSize; i++)
            {
                _mobHandlers.Add(new MobHandler(
                    points => Rewarded?.Invoke(points)));
            }
        }


        protected override sealed void OnActivated(TMob model)
        {
            var handler = _mobHandlers.FirstOrDefault(x => !x.Busy);
            if (handler == null)
            {
                throw new InvalidOperationException("There is no free handler");
            }
            handler.Register(model);
        }


        protected override void OnDisposed()
        {
            foreach (var handler in _mobHandlers)
            {
                handler.Dispose();
            }
        }



        private class MobHandler : IDisposable
        {
            public bool Busy => _model != null;

            private TMob _model;
            private readonly IRewardProvider.RewardedAction _rewardedCallback;


            public MobHandler(IRewardProvider.RewardedAction rewardedCallback)
            {
                _rewardedCallback = rewardedCallback;
            }


            public void Register(TMob mob)
            {
                if (Busy)
                {
                    throw new InvalidOperationException("Handler is budy");
                }
                _model = mob;

                _model.Died += OnDied;
            }


            public void Dispose()
            {
                if (Busy)
                {
                    _model.Died -= OnDied;
                    _model = null;
                }
            }


            private void OnDied()
            {
                _rewardedCallback.Invoke(_model.RewardPoints);
                Dispose();
            }
        }
    }
}
