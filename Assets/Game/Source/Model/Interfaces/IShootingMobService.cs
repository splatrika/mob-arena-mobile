using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IShootingMobService
    {
        IDamageable Spawn(int kindId, Vector3 position, Vector3 shootingPosition);
    }
}
