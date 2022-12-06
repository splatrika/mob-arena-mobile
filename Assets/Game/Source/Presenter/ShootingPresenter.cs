using System;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    [Serializable]
    public class ShootingPresenter : IDisposable
    {
        public bool Inited { get; private set; }

        [SerializeField]
        private string _gunTakenState;

        [SerializeField]
        private string _shotState;

        [SerializeField]
        private string _gunHidenState;

        private IShooting _model;
        private Animator _animator;
        private Transform _transform;


        public void Init(
            IShooting model,
            Animator animator,
            Transform transform)
        {
            if (Inited)
            {
                throw new InvalidOperationException("Already inited");
            }
            _model = model;
            _animator = animator;
            _transform = transform;

            _model.GunTaken += OnGunTaken;
            _model.Shot += OnShot;
            _model.GunHidden += OnGunHiden;

            if (_model.IsGunTaken)
            {
                OnGunTaken();
            }

            Inited = true;
        }


        public void Dispose()
        {
            if (!Inited)
            {
                return;
            }
            _model.GunTaken -= OnGunTaken;
            _model.Shot -= OnShot;
            _model.GunHidden -= OnGunHiden;
            OnGunHiden();

            Inited = false;
        }


        private void OnGunTaken()
        {
            _animator.Play(_gunTakenState);
        }


        private void OnShot(Vector3 direction)
        {
            _animator.Play(_shotState);
            var lookDirction = _transform.position + direction;
            _transform.LookAt(lookDirction);
        }


        private void OnGunHiden()
        {
            _animator.Play(_gunHidenState);
        }
    }
}
