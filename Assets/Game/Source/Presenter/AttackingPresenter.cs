using System;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    [Serializable]
    public class AttackingPresenter : IDisposable
    {
        public bool Inited { get; private set; }

        [SerializeField]
        private string _attackState;

        private IAttacking _model;
        private Animator _animator;


        public void Init(IAttacking model, Animator animator)
        {
            if (Inited)
            {
                throw new InvalidOperationException("Already inited");
            }
            _model = model;
            _animator = animator;

            _model.Attacked += OnAttacked;

            Inited = true;
        }


        public void Dispose()
        {
            if (!Inited)
            {
                return;
            }
            _model.Attacked += OnAttacked;

            Inited = false;
        }


        private void OnAttacked()
        {
            _animator.Play(_attackState);
        }
    }
}
