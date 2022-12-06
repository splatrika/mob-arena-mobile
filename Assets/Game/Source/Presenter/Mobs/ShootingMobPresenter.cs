using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class ShootingMobPresenter
        : MobPresenter<ShootingMob, ShootingMobConfiguration>
    {
        [SerializeField]
        private ShootingPresenter _shootingPresenter;


        protected override void OnInit()
        {
            _shootingPresenter.Init(Model, Animator, transform);
        }


        protected override void OnDispose()
        {
            _shootingPresenter.Dispose();
        }
    }
}
