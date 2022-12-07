using System;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    [Serializable]
    public class DamageablePresenter : IDisposable
    {
        public bool Inited { get; private set; }

        [SerializeField]
        private string _damagedState;

        [SerializeField]
        private string _diedState;

        private Animator _animator;
        private IDamageable _model;


        public void Init(IDamageable model, Animator animator)
        {
            if (Inited)
            {
                throw new InvalidOperationException("Already inited");
            }
            _animator = animator;
            _model = model;

            _model.Damaged += OnDamaged;
            _model.Died += OnDied;
        }


        public void Dispose()
        {
            if (Inited)
            {
                _model.Damaged -= OnDamaged;
                _model.Died -= OnDied;
            }
            Inited = false;
        }


        private void OnDamaged()
        {
            _animator.Play(_damagedState);
        }


        private void OnDied()
        {
            _animator.Play(_diedState);
        }
    }
}
