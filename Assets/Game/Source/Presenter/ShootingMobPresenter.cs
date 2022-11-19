using System;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class ShootingMobPresenter : MonoBehaviour, IReusable<ShootingMob>,
        IPresenter
    {
        public object Model => _model;
        public bool Active { get; private set; }

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private string _idleState;

        [SerializeField]
        private string _movementState;

        [SerializeField]
        private string _takeGunState;

        [SerializeField]
        private string _shootState;

        [SerializeField]
        private string _damagedState;

        [SerializeField]
        private string _diedState;

        private bool _moving;
        private int _lastHealth = 0;
        private Transform _transform;
        private ShootingMob _model;

        public event Action Activated;
        public event Action Deactivated;


        public void Init(ShootingMob model)
        {
            ((IReusable<ShootingMob>)this).Start(model);
        }


        void IReusable<ShootingMob>.Start(ShootingMob configuration)
        {
            if (Active)
            {
                throw new InvalidOperationException("Already active");
            }
            _model = configuration;
            _model.MovementStarted += OnMovementStarted;
            _model.MovementStopped += OnMovementStopped;
            _model.DirectionUpdated += OnDirectionUpdated;
            _model.ShootingStarted += OnShootingStarted;
            _model.Shot += OnShot;
            _model.HealthUpdated += OnHealthUpdated;
            _model.Died += OnDied;

            if (!_transform)
            {
                _transform = transform;
            }

            _lastHealth = _model.Health;

            Active = true;
            Activated?.Invoke();
        }


        private void OnDestroy()
        {
            if (_model != null)
            {
                _model.MovementStarted -= OnMovementStarted;
                _model.MovementStopped -= OnMovementStopped;
                _model.DirectionUpdated -= OnDirectionUpdated;
                _model.ShootingStarted -= OnShootingStarted;
                _model.Shot -= OnShot;
                _model.HealthUpdated -= OnHealthUpdated;
                _model.Died -= OnDied;
            }
        }


        private void Update()
        {
            if (_moving)
            {
                _transform.position = _model.Position;
            }
        }


        private void OnMovementStarted()
        {
            _moving = true;
            _animator.Play(_movementState);
        }


        private void OnMovementStopped()
        {
            _moving = false;
            _animator.Play(_idleState);
        }


        private void OnDirectionUpdated(Vector3 direction)
        {
            var lookAtPoint = _transform.position + direction;
            _transform.LookAt(lookAtPoint);
        }


        private void OnShootingStarted()
        {
            _animator.Play(_takeGunState);
        }


        private void OnShot()
        {
            _animator.Play(_shootState);
        }


        private void OnHealthUpdated(int health)
        {
            if (_lastHealth > health)
            {
                _animator.Play(_damagedState);
            }
            _lastHealth = health;
        }


        private void OnDied()
        {
            _animator.Play(_diedState);

            OnDestroy();
            _model = null;
            Active = false;
            Deactivated?.Invoke();
        }
    }
}
