using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class Bullet : IReusable<BulletConfiguration>, IDamager, IUpdatable,
        IFixedUpdatable
    {
        public Vector3 Position { get; private set; }
        public Vector3 Direction { get; private set; }
        public float Speed { get; private set; }
        public int DamageAmount { get; private set; }
        public bool Active { get; private set; }

        private Vector3 _lastPosition;
        private int _layerMask;
        private float _lifeTime;
        private float _time;
        private readonly IRaycastService _raycastService;
        private readonly ITimeScaleService _timeScaleService;

        public event Action Activated;
        public event Action Deactivated;


        public Bullet(
            float lifeTime,
            int layerMask,
            IRaycastService raycastService,
            ITimeScaleService timeScaleService)
        {
            _lifeTime = lifeTime;
            _layerMask = layerMask;
            _raycastService = raycastService;
            _timeScaleService = timeScaleService;
        }


        public void Start(BulletConfiguration configuration)
        {
            if (Active)
            {
                throw new InvalidOperationException("Already active");
            }
            Active = true;
            Position = configuration.Position;
            _lastPosition = configuration.Position;
            DamageAmount = configuration.Damage;
            Speed = configuration.Speed;
            Direction = configuration.Direction;
            _time = 0;
            Activated?.Invoke();
        }


        public void Update(float deltaTime)
        {
            if (Active)
            {
                Position += Speed * Direction * deltaTime
                    * _timeScaleService.TimeScale;
                _time += deltaTime;
                if (_time >= _lifeTime)
                {
                    Deactivate();
                }
            }
        }


        public void FixedUpdate(float deltaTime)
        {
            if (!Active)
            {
                return;
            }
            var from = _lastPosition;
            var to = Position;
            if (_raycastService.Raycast(from, to, _layerMask,
                out object hitted))
            {
                if (hitted is IDamagable damagable)
                {
                    damagable.Damage(this);
                    Deactivate();
                }
            }
            _lastPosition = Position;
        }


        private void Deactivate()
        {
            Active = false;
            Deactivated?.Invoke();
        }
    }
}
