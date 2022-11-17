using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class BulletPresenter : MonoBehaviour
    {
        private Bullet _model;
        private Transform _transform;


        public void Init(Bullet model)
        {
            _model = model;
            _transform = transform;

            _model.Activated += OnActivated;
            _model.Deactivated += OnDeactivated;

            gameObject.SetActive(_model.Active);
        }


        private void OnDestroy()
        {
            _model.Activated -= OnActivated;
            _model.Deactivated -= OnDeactivated;
        }


        private void Update()
        {
            _transform.position = _model.Position;
        }


        private void OnActivated()
        {
            gameObject.SetActive(true);
        }


        private void OnDeactivated()
        {
            gameObject.SetActive(false);
        }
    }
}
