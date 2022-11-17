using System.Collections.Generic;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class Updater : MonoBehaviour
    {
        private List<IUpdatable> _updatable;
        private List<IFixedUpdatable> _fixedUpdatable;


        [Inject]
        public void Init(
            List<IUpdatable> updatable,
            List<IFixedUpdatable> fixedUpdatable)
        {
            _updatable = updatable;
            _fixedUpdatable = fixedUpdatable;
        }


        private void Update()
        {
            foreach (var model in _updatable)
            {
                model.Update(Time.deltaTime);
            }
        }


        private void FixedUpdate()
        {
            foreach (var model in _fixedUpdatable)
            {
                model.FixedUpdate(Time.fixedDeltaTime);
            }
        }
    }
}
