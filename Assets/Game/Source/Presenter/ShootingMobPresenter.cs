using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class ShootingMobPresenter : MonoBehaviour, IPresenter
    {
        public object Model => _model;

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

        private int _lastHealth = 0;
        private Transform _transform;
        private ShootingMob _model;


        public void Init(ShootingMob model)
        {
            OnDestroy();
            _model = model;
            _transform = transform;

            _lastHealth = _model.Health;

            _model.MovementStarted += OnMovementStarted;
            _model.MovementStopped += OnMovementStopped;
            _model.DirectionUpdated += OnDirectionUpdated;
            _model.ShootingStarted += OnShootingStarted;
            _model.Shot += OnShot;
            _model.HealthUpdated += OnHealthUpdated;
            _model.Died += OnDied;
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


        private void OnMovementStarted()
        {
            _animator.Play(_movementState);
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
        }
    }
}
