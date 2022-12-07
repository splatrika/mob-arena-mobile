using Moq;
using NUnit.Framework;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.UnitTests
{
    public class ShootingStrategyTests
    {
        private ShootingStrategy _strategy;
        private Mock<ITimeScaleService> _timeScaleServiceMock;
        private Mock<IBulletService> _bulletsServiceMock;
        private Mock<IPositionProvider> _shootPointMock;
        private Mock<IPositionProvider> _targetMock;
        private int _bulletsDamage;
        private float _bulletsSpeed;
        private float _regenerationTime;
        private BulletConfiguration? _lastBullet;
        private int _shots;


        [SetUp]
        public void Init()
        {
            _timeScaleServiceMock = new Mock<ITimeScaleService>();
            _bulletsServiceMock = new Mock<IBulletService>();
            _shootPointMock = new Mock<IPositionProvider>();
            _targetMock = new Mock<IPositionProvider>();

            _strategy = new ShootingStrategy(_bulletsServiceMock.Object,
                _timeScaleServiceMock.Object);

            _timeScaleServiceMock.Setup(x => x.TimeScale)
                .Returns(1);
            _bulletsServiceMock
                .Setup(x => x.Spawn(It.IsAny<BulletConfiguration>()))
                .Callback<BulletConfiguration>(x =>
                {
                    _lastBullet = x;
                    _shots++;
                });
            _shootPointMock.Setup(x => x.Position)
                .Returns(new Vector3(2, 5.5f, 12));
            _targetMock.Setup(x => x.Position)
                .Returns(new Vector3(10, 5.5f, -3.1f));
            _bulletsDamage = 12;
            _bulletsSpeed = 5;
            _regenerationTime = 12;

            _lastBullet = null;
            _shots = 0;

            _strategy.Setup(_bulletsDamage, _bulletsSpeed,
                _shootPointMock.Object, _targetMock.Object, _regenerationTime);
        }


        [Test]
        public void ShouldStartShooting()
        {
            _strategy.Start();
            _strategy.Update(_regenerationTime / 2);
            _strategy.Update(_regenerationTime / 2);
            var exceptedDirection = _targetMock.Object.Position
                - _shootPointMock.Object.Position;

            Assert.AreEqual(2, _shots);
            Assert.NotNull(_lastBullet);
            Assert.AreEqual(_shootPointMock.Object.Position,
                _lastBullet.Value.Position);
            Assert.AreEqual(exceptedDirection, _lastBullet.Value.Direction);
            Assert.AreEqual(_bulletsDamage, _lastBullet.Value.Damage);
            Assert.AreEqual(_bulletsSpeed, _lastBullet.Value.Speed);
        }


        [Test]
        public void ShouldStopShooting()
        {
            _strategy.Start();
            _strategy.Update(_regenerationTime / 2);
            _strategy.Stop();
            _strategy.Update(_regenerationTime / 2);

            Assert.AreEqual(1, _shots);
        }


        [Test]
        public void ShouldBeAffectedByTimeScale()
        {
            var timeScale = 4;
            _timeScaleServiceMock.Setup(x => x.TimeScale)
                .Returns(timeScale);

            _strategy.Start();
            _strategy.Update(_regenerationTime / 2 / timeScale);
            _strategy.Update(_regenerationTime / 2 / timeScale);

            Assert.AreEqual(2, _shots);
        }
    }
}
