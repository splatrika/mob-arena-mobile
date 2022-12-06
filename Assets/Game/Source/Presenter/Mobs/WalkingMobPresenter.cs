using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    public class WalkingMobPresenter
        : MobPresenter<WalkingMob, WalkingMobConfiguration>
    {
        [SerializeField]
        private AttackingPresenter _attackingPresenter;


        protected override void OnInit()
        {
            _attackingPresenter.Init(Model, Animator);
        }


        protected override void OnDispose()
        {
            _attackingPresenter.Dispose();
        }
    }
}
