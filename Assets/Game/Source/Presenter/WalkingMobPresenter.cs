using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class WalkingMobPresenter : MonoBehaviour, IPresenter
    {
        public object Model => _model;

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


        public void Init(WalkingMob model)
        {
            _model = model;
            _model.MovementStarted += OnMovementStarted;
            _model.MovementStopped += OnMovementStopped;
            _model.DirectionUpdated += OnDirectionUpdated;
            _model.Atacked += OnAtacked;

            _transform = transform;
        }


        private void OnDestroy()
        {
            _model.MovementStarted -= OnMovementStarted;
            _model.MovementStopped -= OnMovementStopped;
            _model.DirectionUpdated -= OnDirectionUpdated;
            _model.Atacked -= OnAtacked;
        }


        private void Update()
        {
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
    }
}
