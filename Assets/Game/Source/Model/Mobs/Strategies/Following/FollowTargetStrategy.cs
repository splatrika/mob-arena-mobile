using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class FollowTargetStrategy : IFollowingStrategy, IDisposable
    {
        public const float CheckTargetFrequency = 1.5f;
        public Vector3 Position => _movement.Position;

        private NavigatedMovement _movement;
        private IPositionProvider _target;
        private Vector3 _lastTargetPosition;
        private RegeneratingAction _targetChecking;

        public event Action Arrived;
        public event Action StartedFollowing;
        public event Action<Vector3> DirectionUpdated;


        public FollowTargetStrategy(
            NavigationConfiguration configuration,
            INavigationService navigationService,
            ILogger logger,
            ITimeScaleService timeScaleService)
        {
            _movement = new NavigatedMovement(configuration,
                navigationService, logger, timeScaleService);

            _targetChecking = new RegeneratingAction(
                action: CheckTarget,
                timeScaleService: timeScaleService,
                regenerationTime: CheckTargetFrequency);

            _movement.Arrived += OnArrived;
            _movement.MovementStarted += OnMovementStarted;
            _movement.DirectionUpdated += OnDirectionUpdated;
        }


        public void Dispose()
        {
            _movement.Arrived -= OnArrived;
            _movement.MovementStarted -= OnMovementStarted;
            _movement.DirectionUpdated -= OnDirectionUpdated;
        }


        public void Setup(IPositionProvider target, float speed,
            float minDistance)
        {
            _target = target;
            _movement.Configure(speed, minDistance);
            _movement.SetDestination(_target.Position);
            _targetChecking.Start();
        }


        public void SetPosition(Vector3 position)
        {
            _movement.SetPosition(position);
        }


        public void Stop()
        {
            _target = null;
            _movement.ClearDestination();
            _targetChecking.Stop();
        }


        public void Update(float deltaTime)
        {
            _movement.Update(deltaTime);
            _targetChecking.Update(deltaTime);
        }


        private void CheckTarget()
        {
            if (_lastTargetPosition != _target.Position)
            {
                _movement.SetDestination(_target.Position);
                _lastTargetPosition = _target.Position;
            }
        }


        private void OnArrived()
        {
            Arrived?.Invoke();
        }


        private void OnMovementStarted()
        {
            StartedFollowing?.Invoke();
        }


        private void OnDirectionUpdated(Vector3 position)
        {
            DirectionUpdated?.Invoke(position);
        }
    }
}
