using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class GoToPointStrategy : IFollowingStrategy, IDisposable
    {
        public Vector3 Position => _movement.Position;

        private NavigatedMovement _movement;

        public event Action Arrived;
        public event Action StartedFollowing;
        public event Action<Vector3> DirectionUpdated;


        public GoToPointStrategy(
            NavigationConfiguration configuration,
            INavigationService navigationService,
            ILogger logger,
            ITimeScaleService timeScaleService)
        {
            _movement = new NavigatedMovement(configuration,
                navigationService, logger, timeScaleService);

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


        public void Setup(Vector3 target, float speed)
        {
            _movement.Configure(speed: speed, minDistance: 0);
            _movement.SetDestination(target);
        }


        public void SetPosition(Vector3 position)
        {
            _movement.SetPosition(position);
        }


        public void Stop()
        {
            _movement.ClearDestination();
        }


        public void Update(float deltaTime)
        {
            _movement.Update(deltaTime);
        }


        private void OnArrived()
        {
            Arrived?.Invoke();
        }


        private void OnMovementStarted()
        {
            StartedFollowing?.Invoke();
        }


        private void OnDirectionUpdated(Vector3 direction)
        {
            DirectionUpdated?.Invoke(direction);
        }
    }
}
