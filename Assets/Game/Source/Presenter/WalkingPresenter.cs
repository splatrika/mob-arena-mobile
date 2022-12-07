using System;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    [Serializable]
    public class WalkingPresenter : IDisposable, IUpdatable
    {
        public bool Inited { get; private set; }

        [SerializeField]
        private string _idleState;

        [SerializeField]
        private string _walkingState;

        private IWalking _model;
        private Animator _animator;
        private Transform _transform;


        public void Init(
            IWalking model,
            Animator animator,
            Transform transform)
        {
            if (Inited)
            {
                throw new InvalidOperationException("Already inited");
            }
            _model = model;
            _animator = animator;
            _transform = transform;

            _model.MovementStarted += OnStarted;
            _model.MovementStopped += OnStopped;
            _model.DirectionUpdated += OnDirectionUpdated;

            if (_model.IsWalking)
            {
                OnStarted();
            }
            OnDirectionUpdated(_model.Direction);

            Inited = true;
        }


        public void Update(float deltaTime)
        {
            _transform.position = _model.Position;
        }


        public void Dispose()
        {
            if (!Inited)
            {
                return;
            }
            _model.MovementStarted -= OnStarted;
            _model.MovementStopped -= OnStopped;
            _model.DirectionUpdated -= OnDirectionUpdated;
            OnStopped();

            Inited = false;
        }


        private void OnStarted()
        {
            _animator.Play(_walkingState);
        }


        private void OnStopped()
        {
            _animator.Play(_idleState);
        }


        private void OnDirectionUpdated(Vector3 direction)
        {
            var lookPoint = _transform.position + direction;
            _transform.LookAt(lookPoint);
        }
    }
}
