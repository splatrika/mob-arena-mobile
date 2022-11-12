using System.Collections.Generic;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class Updater : MonoBehaviour
    {
        private List<IUpdatable> _models;


        [Inject]
        public void Init(List<IUpdatable> models)
        {
            _models = models;
        }


        private void Update()
        {
            foreach (var model in _models)
            {
                model.Update(Time.deltaTime);
            }
        }
    }
}
