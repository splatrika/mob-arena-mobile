using System;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    public abstract class MobPresenter<TMob, TConfiguration>
        : MonoBehaviour, IPresenter
        where TMob : Mob<TConfiguration>
        where TConfiguration : MobConfiguration
    {
        protected TMob Model { get; private set; }
        protected Animator Animator => _animator;

        [SerializeField]
        private WalkingPresenter _walkingPresenter;

        [SerializeField]
        private DamageablePresenter _damageablePresenter;

        [SerializeField]
        private Animator _animator;

        object IPresenter.Model => Model;

        protected abstract void OnInit();
        protected abstract void OnDispose();
        protected virtual void OnUpdate() { }


        public void Init(TMob mob)
        {
            if (Model != null)
            {
                throw new InvalidOperationException("This presenter is busy");
            }
            if (!mob.Active)
            {
                throw new InvalidOperationException("The mob is not active");
            }
            Model = mob;

            Model.Deactivated += OnDestroy;
            _walkingPresenter.Init(Model, _animator, transform);
            _damageablePresenter.Init(Model, _animator);
            OnInit();
        }


        private void Update()
        {
            _walkingPresenter.Update(Time.deltaTime);
            OnUpdate();
        }


        private void OnDestroy()
        {
            if (Model != null)
            {
                Model.Deactivated -= OnDestroy;
                _walkingPresenter.Dispose();
                _damageablePresenter.Dispose();
                OnDispose();
                Model = null;
            }
        }
    }
}
