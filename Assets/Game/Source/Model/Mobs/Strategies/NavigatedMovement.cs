using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class NavigatedMovement : IUpdatable
    {
        public bool Active => _target != null;
        public Vector3 Position => _position;
        public Vector3? Target => _target;
        public float Speed => _speed;
        public float MinDistance => _minDistance;

        private Vector3? _target;
        private Vector3 _position;
        private float _speed;
        private float _minDistance;

        private Vector3[] _pathBuffer;
        private float[] _edgeLengths;
        private float _totalPathLength;
        private int _points;
        private int _edges => _points - 1;
        private float _time;
        private int _lastEdge;

        private readonly INavigationService _service;
        private readonly ILogger _logger;
        private readonly ITimeScaleService _timeScaleService;

        public event Action MovementStarted;
        public event Action MovementStopped;
        public event Action Arrived;
        public event Action<Vector3> DirectionUpdated;


        public NavigatedMovement(
            NavigationConfiguration configuration,
            INavigationService service,
            ILogger logger,
            ITimeScaleService timeScaleService)
        {
            _service = service;
            _logger = logger;
            _timeScaleService = timeScaleService;

            _pathBuffer = new Vector3[configuration.PathBuffersSize];
            _edgeLengths = new float[configuration.PathBuffersSize - 1];
        }


        public void SetPosition(Vector3 position)
        {
            _position = position;
            if (Active)
            {
                SetDestination((Vector3)_target);
            }
        }


        public void SetDestination(Vector3 target)
        {
            _target = target;
            _time = 0;
            _lastEdge = -1;
            if (!_service.TryGetPath(_position, target, _pathBuffer,
                out _points))
            {
                _logger.LogWarning(nameof(NavigatedMovement),
                    "There is no way to go after target");
                return;
            }
            _totalPathLength = 0;
            for (int i = 0; i < _edges; i++)
            {
                var a = _pathBuffer[i];
                var b = _pathBuffer[i + 1];
                _edgeLengths[i] = Vector3.Distance(a, b);
                _totalPathLength += _edgeLengths[i];
            }
            MovementStarted?.Invoke();
        }


        public void ClearDestination()
        {
            if (!Active)
            {
                return;
            }
            _target = null;
            MovementStopped?.Invoke();
        }


        public void Configure(float speed, float minDistance)
        {
            _speed = speed;
            _minDistance = minDistance;
        }


        public void Update(float deltaTime)
        {
            if (Active)
            {
                _time += _speed * deltaTime * _timeScaleService.TimeScale;
                if (_time > _totalPathLength)
                {
                    if (_minDistance == 0f)
                    {
                        _position = (Vector3)_target;
                        Arrived?.Invoke();
                        ClearDestination();
                        return;
                    }
                    else
                    {
                        _time = _totalPathLength;
                    }
                }
                var currentEdge = GetCurrentEdge(_time,
                    out float _lengthBeforCurrentEdge);

                var a = _pathBuffer[currentEdge];
                var b = _pathBuffer[currentEdge + 1];
                var lerpTime = (_time - _lengthBeforCurrentEdge)
                    / _edgeLengths[currentEdge];
                var _lastPosition = _position;
                _position = Vector3.Lerp(a, b, lerpTime);

                if (_lastEdge != currentEdge)
                {
                    var direction = b - a;
                    direction = direction.normalized;
                    DirectionUpdated?.Invoke(direction);
                    _lastEdge = currentEdge;
                }

                if (_minDistance == 0f)
                {
                    return;
                }
                var distance = Vector3
                    .Distance(_position, (Vector3)_target);
                if (distance <= _minDistance)
                {
                    var direction = (Vector3)_target - _lastPosition;
                    direction = direction.normalized;
                    _position = (Vector3)_target - (direction * _minDistance);
                    Arrived?.Invoke();
                    ClearDestination();
                }
            }
        }


        private int GetCurrentEdge(float time,
            out float pathLengthBeforeCurrentEdge)
        {
            var totalLength = 0f;
            for (int i = 0; i < _edges; i++)
            {
                totalLength += _edgeLengths[i];
                if (time <= totalLength)
                {
                    pathLengthBeforeCurrentEdge = totalLength - _edgeLengths[i];
                    return i;
                }
            }
            throw new ArgumentOutOfRangeException("Time is out of path length");
        }
    }
}
