using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class WalkingMobSpawnPoint : MobSpawnPoint
    {
        public int KindId { get; }
        public Vector3 Position { get; }

        private readonly IWalkingMobService _walkingMobService;


        public WalkingMobSpawnPoint(
            float spawnTime,
            int kindId,
            Vector3 position,
            IWalkingMobService walkingMobService)
            : base(spawnTime)
        {
            KindId = kindId;
            Position = position;

            _walkingMobService = walkingMobService;
        }


        public override IDamageable Spawn()
        {
            return _walkingMobService.Spawn(KindId, Position);
        }
    }
}
