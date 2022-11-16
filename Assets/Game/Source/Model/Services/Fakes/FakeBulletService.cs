using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class FakeBulletService : IBulletService
    {
        public void Spawn(BulletConfiguration configuration)
        {
            Debug.Log("Shot!");
        }
    }
}
