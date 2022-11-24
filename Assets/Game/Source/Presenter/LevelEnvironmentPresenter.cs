using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class LevelEnvironmentPresenter : MonoBehaviour, IPresenter
    {
        public object Model { get; private set; }


        public void Init(ILevelEnvironment model)
        {
            Model = model;
        }
    }
}
