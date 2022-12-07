using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public abstract class FollowingStrategyBase : IFollowingStrategy
    {
        public abstract Vector3 Position { get; }

        public abstract event Action Arrived;
        public abstract event Action StartedFollowing;
        public abstract event Action<Vector3> DirectionUpdated;
        public event Action Frozen;
        public event Action Resumed;

        public abstract void SetPosition(Vector3 position);

        protected abstract bool IsActive();
        protected abstract void OnStop();
        protected abstract void OnUpdate(float deltaTime);
        protected virtual void OnHit() { }
        protected virtual void OnRegenerated() { }

        private float _regenerationTime;
        private float _regenerationTimeLeft;


        public void Hit()
        {
            if (!IsActive())
            {
                return;
            }
            _regenerationTimeLeft = _regenerationTime;
            Frozen?.Invoke();
            OnHit();
        }


        public void SetRegenerationTime(float time)
        {
            _regenerationTime = time;
        }


        public void Stop()
        {
            OnStop();
            _regenerationTimeLeft = 0;
        }


        public void Update(float deltaTime)
        {
            if (_regenerationTimeLeft > 0)
            {
                _regenerationTimeLeft -= deltaTime;
                if (_regenerationTimeLeft <= 0 && IsActive())
                {
                    Resumed?.Invoke();
                }
                return;
            }
            OnUpdate(deltaTime);
        }
    }
}
