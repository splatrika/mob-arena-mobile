using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IWalkingMobService
    {
        IHealth Spawn(int kindId, Vector3 position);
    }
}
