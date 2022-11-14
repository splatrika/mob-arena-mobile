using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IPlayerCharacter: IDamagable, IHealth
    {
        Vector3 Position { get; }
    }
}
