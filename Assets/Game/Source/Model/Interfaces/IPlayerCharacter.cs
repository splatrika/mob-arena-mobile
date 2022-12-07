using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public interface IPlayerCharacter: IDamageable, IPositionProvider
    {
        Vector3 CenterOffset { get; }
        Vector3 Center { get; }

        void SetDirection(Vector2 direction);
        void StartShooting();
        void StopShooting();
    }
}
