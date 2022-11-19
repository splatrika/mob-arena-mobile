using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    public interface IReusablePresenterProvider
    {
        IActiveStatus Presenter { get; }
    }
}
