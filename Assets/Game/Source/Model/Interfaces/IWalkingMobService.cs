using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IWalkingMobService
    {
        IDamageable Spawn(int kindId, Vector3 position);
    }
}
