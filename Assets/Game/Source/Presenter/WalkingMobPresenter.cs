using System;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class WalkingMobPresenter : MonoBehaviour, IReusable<WalkingMob>,
        IPresenter
    {
        public object Model => _model;
        public bool Active { get; private set; }

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private string _walkingState;

        [SerializeField]
        private string _idleState;

        [SerializeField]
        private string _atackState;

        private WalkingMob _model;
        private Transform _transform;

        public event Action Activated;
        public event Action Deactivated;


        public void Init(WalkingMob model)
        {
            ((IReusable<WalkingMob>)this).Start(model);
        }


        void IReusable<WalkingMob>.Start(WalkingMob configuration)
        {
            if (Active)
            {
                throw new InvalidOperationException("Already active");
            }
            gameObject.SetActive(true);

            _model = configuration;
            _model.MovementStarted += OnMovementStarted;
            _model.MovementStopped += OnMovementStopped;
            _model.DirectionUpdated += OnDirectionUpdated;
            _model.Atacked += OnAtacked;
            _model.Died += OnDied;

            if (!_transform)
            {
                _transform = transform;
            }

            if (_model.IsMoving)
            {
                OnMovementStarted();
            }

            Active = true;
            Activated?.Invoke();
        }


        private void Awake()
        {
            if (_model == null)
            {
                gameObject.SetActive(false);
            }
        }


        private void OnDestroy()
        {
            if (_model != null)
            {
                _model.MovementStarted -= OnMovementStarted;
                _model.MovementStopped -= OnMovementStopped;
                _model.DirectionUpdated -= OnDirectionUpdated;
                _model.Atacked -= OnAtacked;
                _model.Died -= OnDied;
            }
        }


        private void Update()
        {
            if (_model == null)
            {
                return;
            }
            _transform.position = _model.Position;
        }


        private void OnMovementStarted()
        {
            _animator.Play(_walkingState);
        }


        private void OnMovementStopped()
        {
            _animator.Play(_idleState);
        }


        private void OnDirectionUpdated(Vector3 direction)
        {
            var lookAtPoint = _transform.position + direction;
            _transform.LookAt(lookAtPoint);
        }


        private void OnAtacked()
        {
            _animator.Play(_atackState);
        }


        private void OnDied()
        {
            OnDestroy();
            _model = null;
            Active = false;
            Deactivated?.Invoke();
        }
    }
}
