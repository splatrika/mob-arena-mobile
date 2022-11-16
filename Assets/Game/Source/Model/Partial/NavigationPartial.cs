using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class NavigationPartial : INavigationPartial
    {
        public Vector3 Position { get; private set; }

        private bool _active;
        private Vector3 _target;
        private Vector3[] _pathBuffer;
        private float[] _edgeLengths;
        private float _totalPathLength;
        private int _points;
        private int _edges => _points - 1;
        private float _time;
        private float _speed;
        private readonly INavigationService _service;
        private readonly ILogger _logger;
        private readonly ITimeScaleService _timeScaleService;


        public NavigationPartial(
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


        public bool Start(Vector3 start, Vector3 target, float speed)
        {
            Position = start;
            if (!_service.TryGetPath(start, target, _pathBuffer, out _points))
            {
                _logger.LogWarning(nameof(NavigationPartial),
                    "There is no path between given two points");
                return false;
            }
            _totalPathLength = 0;
            for (int i = 0; i < _edges; i++)
            {
                var a = _pathBuffer[i];
                var b = _pathBuffer[i + 1];
                _edgeLengths[i] = Vector3.Distance(a, b);
                _totalPathLength += _edgeLengths[i];
            }
            _speed = speed;
            _time = 0;
            _active = true;
            _target = target;
            return true;
        }


        public void Stop()
        {
            _active = false;
        }


        public void Update(float deltaTime)
        {
            if (_active)
            {
                _time += _speed * deltaTime * _timeScaleService.TimeScale;
                if (_time > _totalPathLength)
                {
                    Position = _target;
                    _active = false;
                    return;
                }
                var currentEdge = GetCurrentEdge(_time,
                    out float _lengthBeforCurrentEdge);

                var a = _pathBuffer[currentEdge];
                var b = _pathBuffer[currentEdge + 1];
                var lerpTime = (_time - _lengthBeforCurrentEdge)
                    / _edgeLengths[currentEdge];
                Position = Vector3.Lerp(a, b, lerpTime);
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
