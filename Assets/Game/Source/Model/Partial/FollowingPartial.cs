using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class FollowingPartial : IFollowingPartial
    {
        public Vector3 Position { get; private set; }

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
            _navigationPartial.Stop();
        }


        public void Start(FollowingConfiguration configuration)
        {
            _configuration = configuration;
            if (_navigationPartial.Start(_configuration.Start,
                _configuration.Target, _configuration.Speed))
            {
                MovementStarted?.Invoke();
                _started = true;
            }
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
                _navigationPartial.Stop();
                Arrived?.Invoke();
            }

            if (minDistance != 0 &&
                Vector3.Distance(Position, _configuration.Target) < minDistance)
            {
                direction = _configuration.Target - Position;
                direction = direction.normalized;
                _navigationPartial.Stop();
                _started = false;
                Position = _configuration.Target
                    + (direction * -1 * minDistance);
                Arrived?.Invoke();
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
                _navigationPartial.Start(_navigationPartial.Position,
                    _configuration.Target, _configuration.Speed);
            }
        }
    }
}
