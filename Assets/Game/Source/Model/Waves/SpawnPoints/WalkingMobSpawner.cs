using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class WalkingMobSpawner : MobSpawnPoint
    {
        public int KindId { get; }
        public Vector3 Position { get; }

        private readonly IWalkingMobService _walkingMobService;


        public WalkingMobSpawner(
            WalkingMobSpawnerConfiguration configuration,
            IWalkingMobService walkingMobService)
            : base(configuration.SpawnTime)
        {
            KindId = configuration.KindId;
            Position = configuration.Position;

            _walkingMobService = walkingMobService;
        }


        public override IHealth Spawn()
        {
            return _walkingMobService.Spawn(KindId, Position);
        }
    }
}
