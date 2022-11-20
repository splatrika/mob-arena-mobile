using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.UI
{
    public class DefaultJoystickControl : JoystickControl
    {
        public override Vector2 Offset => _offset;

        [SerializeField]
        private RectTransform _center;

        [SerializeField]
        private RectTransform _stick;

        [SerializeField]
        private float _maxRadius;

        private bool _touched;
        private Vector2 _offset;

        public override event Action Touched;
        public override event Action Untouched;
        public override event Action<Vector2> OffsetUpdated;


        private void Start()
        {
            _stick.position = _center.position;
        }


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var distance = Vector2.Distance(
                    Input.mousePosition, _center.position);
                if (distance <= _maxRadius)
                {
                    _touched = true;
                    Touched?.Invoke();
                }
            }
            if (Input.GetMouseButtonUp(0) && _touched)
            {
                _touched = false;
                _stick.position = _center.position;
                Untouched?.Invoke();
            }

            if (_touched)
            {
                _offset = Input.mousePosition - _center.position;
                if (_offset.magnitude > _maxRadius)
                {
                    _offset = _offset.normalized * _maxRadius;
                }
                _stick.position = _offset + (Vector2)_center.position;
                _offset *= 1 / _maxRadius;
                OffsetUpdated?.Invoke(_offset);
            }
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_center.position, _maxRadius);
        }
    }
}
