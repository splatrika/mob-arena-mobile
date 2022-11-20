using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class FollowingPartial : IFollowingPartial
    {
        public Vector3 Position { get; private set; }
        public bool IsMoving => _started;

        private bool _started;
        private bool _regenerating;
        private float _regenerationTime;
        private Vector3 _lastDirection;
        private FollowingConfiguration _configuration;
        private readonly INavigationPartial _navigationPartial;
        private readonly ITimeScaleService _timeScaleService;

        public event Action Arrived;
        public event Action MovementStarted;
        public event Action<Vector3> DirectionUpdated;
        public event Action MovementStopped;


        public FollowingPartial(
            INavigationPartial navigationPartial,
            ITimeScaleService timeScaleService)
        {
            _navigationPartial = navigationPartial;
            _timeScaleService = timeScaleService;
        }


        public void Hit()
        {
            _regenerating = true;
            _regenerationTime = 0;
            StopMovement();
        }


        public void Start(FollowingConfiguration configuration)
        {
            _configuration = configuration;
            StartMovement(_configuration.Start, _configuration.Target,
                _configuration.Speed);
        }


        public void Update(float deltaTime)
        {
            UpdateRegenerating(deltaTime);
            UpdateMovement(deltaTime);
        }


        private void UpdateMovement(float deltaTime)
        {
            _navigationPartial.Update(deltaTime);
            if (!_started)
            {
                return;
            }

            var direction = _navigationPartial.Position - Position;
            direction = direction.normalized;
            if (_lastDirection != direction)
            {
                DirectionUpdated?.Invoke(direction);
                _lastDirection = direction;
            }

            Position = _navigationPartial.Position;

            var minDistance = _configuration.MinDistance;
            if (minDistance == 0 && Position == _configuration.Target)
            {
                StopMovement();
                Arrived?.Invoke();
            }
            if (minDistance != 0 &&
                Vector3.Distance(Position, _configuration.Target) < minDistance)
            {
                direction = _configuration.Target - Position;
                direction = direction.normalized;
                StopMovement();
                Position = _configuration.Target
                    + (direction * -1 * minDistance);
                Arrived?.Invoke();
            }
        }


        private void StopMovement()
        {
            _navigationPartial.Stop();
            _started = false;
            MovementStopped?.Invoke();
        }


        private void StartMovement(Vector3 start, Vector3 target, float speed)
        {
            if (_navigationPartial.Start(start, target, speed))
            {
                _started = true;
                MovementStarted?.Invoke();
            }
        }


        private void UpdateRegenerating(float deltaTime)
        {
            if (!_regenerating)
            {
                return;
            }
            _regenerationTime += deltaTime * _timeScaleService.TimeScale;
            if (_regenerationTime >= _configuration.RegenerationTime)
            {
                _regenerating = false;
                StartMovement(_navigationPartial.Position,
                    _configuration.Target, _configuration.Speed);
            }
        }
    }
}
