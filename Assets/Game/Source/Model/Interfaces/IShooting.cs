using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IShooting
    {
        bool IsGunTaken { get; }

        delegate void ShotAction(Vector3 direction);

        event Action GunTaken;
        event ShotAction Shot;
        event Action GunHidden;
    }
}
