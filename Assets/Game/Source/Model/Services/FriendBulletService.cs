using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class FriendBulletService
        : PoolService<BulletConfiguration, FriendBullet>, IFriendBulletService,
        IUpdatable, IFixedUpdatable
    {
        private readonly float _speed;
        private readonly int _damage;
        private readonly float _lifeTime;
        private readonly int _layerMask;
        private readonly IRaycastService _raycastService;
        private readonly ITimeScaleService _timeScaleService;


        public FriendBulletService(
            FriendBulletServiceConfiguration configuration,
            ILogger logger,
            IRaycastService raycastService,
            ITimeScaleService timeScaleService)
            : base(configuration.PoolSize, logger)
        {
            CreateObjects();
            _raycastService = raycastService;
            _timeScaleService = timeScaleService;
            _speed = configuration.Speed;
            _damage = configuration.Damage;
            _lifeTime = configuration.BulletsLifeTime;
            _layerMask = configuration.LayerMask;
            CreateObjects();
        }


        public void Spawn(Vector3 position, Vector3 direction)
        {
            var configuration = new BulletConfiguration(
                position, direction, _speed, _damage);
            Spawn(configuration);
        }


        public void Update(float deltaTime)
        {
            for (var i = 0; i < Objects.Count; i++)
            {
                var bullet = Objects[i];
                if (bullet.Active)
                {
                    bullet.Update(deltaTime);
                }
            }
        }


        public void FixedUpdate(float deltaTime)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                var bullet = Objects[i];
                if (bullet.Active)
                {
                    bullet.FixedUpdate(deltaTime);
                }
            }
        }


        protected override FriendBullet CreateInstance()
        {
            return new FriendBullet(_lifeTime, _layerMask, _raycastService,
                _timeScaleService);
        }
    }
}
