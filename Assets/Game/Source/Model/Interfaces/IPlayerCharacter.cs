using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IPlayerCharacter: IDamagable, IHealth
    {
        Vector3 Position { get; }
        Vector3 CenterOffset { get; }
        Vector3 Center { get; }
    }
}
