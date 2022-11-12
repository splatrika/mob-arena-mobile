using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class FakeFriendBulletService : IFriendBulletService
    {
        private readonly ILogger _logger;

        public void Spawn(Vector3 position, Vector3 direction)
        {
            _logger.Log($"Spawn at {position}. Direction: {direction}");
        }
    }
}
