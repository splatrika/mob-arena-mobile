using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class PlayerCharacterPresenter : MonoBehaviour, IPresenter
    {
        public object Model => _model;

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private string _shootState;

        [SerializeField]
        private string _rotateClockwiseState;

        [SerializeField]
        private string _rotateCounterClockwiseState;

        [SerializeField]
        private string _damagedState;

        [SerializeField]
        private string _diedState;

        private PlayerCharacter _model;
        private Vector3 _lastDirection;
        private float _lastHealth;


        public void Init(PlayerCharacter model)
        {
            _model = model;

            _model.Shot += OnShot;
            _model.Rotated += OnRotated;
            _model.HealthUpdated += OnHealthUpdated;
            _model.Died += OnDied;

            _lastDirection = _model.Direction;
            _lastHealth = _model.Health;
            transform.position = _model.Position;
            OnRotated(_model.Direction);
        }


        private void OnDestroy()
        {
            _model.Shot -= OnShot;
            _model.Rotated -= OnRotated;
            _model.HealthUpdated -= OnHealthUpdated;
            _model.Died -= OnDied;
        }


        private void OnShot()
        {
            _animator.Play(_shootState);
        }


        private void OnRotated(Vector3 direction)
        {
            var lookAtPosition = transform.position + _model.Direction;
            transform.LookAt(lookAtPosition);

            var delta = Mathf.Atan2(direction.x, direction.y)
                - Mathf.Atan2(_lastDirection.x, _lastDirection.y);
            if (delta == 0)
            {
                return;
            }
            var state = delta > 0
                ? _rotateClockwiseState
                : _rotateCounterClockwiseState;
            _animator.Play(state);

            _lastDirection = direction;
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


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPresenter presenter))
            {
                if (presenter.Model is IDamager damager)
                {
                    _model.Damage(damager);
                }
            }
        }
    }
}
