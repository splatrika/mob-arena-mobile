using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class GoToPointStrategy : FollowingStrategyBase, IDisposable
    {
        public override Vector3 Position => _movement.Position;

        private NavigatedMovement _movement;

        public override event Action Arrived;
        public override event Action StartedFollowing;
        public override event Action<Vector3> DirectionUpdated;


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


        public override sealed void SetPosition(Vector3 position)
        {
            _movement.SetPosition(position);
        }


        protected override sealed void OnStop()
        {
            _movement.ClearDestination();
        }


        protected override sealed void OnUpdate(float deltaTime)
        {
            _movement.Update(deltaTime);
        }


        protected override bool IsActive()
        {
            return _movement.Active;
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
