using Moq;
using NUnit.Framework;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.UnitTests
{
    public class BulletTests
    {
        private float _lifeTime;
        private int _layerMask;
        private BulletConfiguration _configuration;
        private Bullet _bullet;
        private Mock<IRaycastService> _raycastServiceMock;
        private Mock<ITimeScaleService> _timeScaleServiceMock;



        [SetUp]
        public void Init()
        {
            _lifeTime = 14;
            _layerMask = 0x1101;

            _raycastServiceMock = new Mock<IRaycastService>();
            _timeScaleServiceMock = new Mock<ITimeScaleService>();

            _bullet = new Bullet(_lifeTime, _layerMask,
                _raycastServiceMock.Object, _timeScaleServiceMock.Object);

            _configuration = new BulletConfiguration(
                position: Vector3.zero,
                direction: Vector3.left,
                speed: 12,
                damage: 1);

            _timeScaleServiceMock.SetupGet(x => x.TimeScale)
                .Returns(1);
        }


        [Test]
        public void ShouldAttackDamager()
        {
            _bullet.Start(_configuration);

            var damagableMock = new Mock<IDamagable>();
            var damagable = (object)damagableMock.Object;
            IDamager damager = null;
            damagableMock.Setup(x => x.Damage(It.IsAny<IDamager>()))
                .Callback((IDamager x) => damager = x);

            var lastPosition = _bullet.Position;
            _bullet.Update(1);
            var position = _bullet.Position;
            _raycastServiceMock
                .Setup(x => x.Raycast(lastPosition, position, _layerMask,
                    out damagable))
                .Returns(true);
            _bullet.FixedUpdate(1);

            Assert.NotNull(damager);
            Assert.AreEqual(_bullet, damager);
        }


        [Test]
        public void ShouldMove()
        {
            _configuration.Position = new Vector3(1, 2, 1);
            _configuration.Direction = new Vector3(0, 1, 0);
            _configuration.Speed = 4;
            var exceptedPosition = new Vector3(1, 10, 1);

            _bullet.Start(_configuration);
            _bullet.Update(2);

            Assert.AreEqual(exceptedPosition, _bullet.Position);
        }


        [Test]
        public void ShouldMoveUsingTimeScale()
        {
            _timeScaleServiceMock.SetupGet(x => x.TimeScale)
                .Returns(2);

            _configuration.Position = new Vector3(1, 3, -2);
            _configuration.Direction = new Vector3(1, 0, 0);
            _configuration.Speed = 3;
            var exceptedPosition = new Vector3(13, 3, -2);

            _bullet.Start(_configuration);
            _bullet.Update(2);

            Assert.AreEqual(exceptedPosition, _bullet.Position);
        }


        [Test]
        public void ShouldBeActivated()
        {
            var activated = false;
            _bullet.Activated += () => activated = true;
            _bullet.Start(_configuration);

            Assert.True(activated);
            Assert.True(_bullet.Active);
        }


        [Test]
        public void ShouldBeDeactivatedOnHit()
        {
            var damagable = (object)new Mock<IDamagable>().Object;

            var deactivated = false;
            _bullet.Deactivated += () => deactivated = true;

            _bullet.Start(_configuration);
            _raycastServiceMock
                .Setup(x => x.Raycast(It.IsAny<Vector3>(), It.IsAny<Vector3>(),
                    It.IsAny<int>(), out damagable))
                .Returns(true);
            _bullet.Update(1);
            _bullet.FixedUpdate(1);

            Assert.True(deactivated);
            Assert.False(_bullet.Active);
        }


        [Test]
        public void ShouldBeDeactivatedInLifeTime()
        {
            var deactivated = false;
            _bullet.Deactivated += () => deactivated = true;

            _bullet.Start(_configuration);
            _bullet.Update(_lifeTime / 2);
            _bullet.Update(_lifeTime / 2);

            Assert.True(deactivated);
            Assert.False(_bullet.Active);
        }
    }
}
