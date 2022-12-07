using Moq;
using NUnit.Framework;
using Splatrika.MobArenaMobile.Model;

namespace Splatrika.MobArenaMobile.UnitTests
{
    public class DirectAttackStrategyTests
    {
        private DirectAttackStrategy _strategy;
        private Mock<ITimeScaleService> _timeScaleServiceMock;
        private Mock<IDamageable> _targetMock;
        private int _damageAmount;
        private float _regenerationTime;
        private int _targetDamage;


        [SetUp]
        public void Init()
        {
            _timeScaleServiceMock = new Mock<ITimeScaleService>();
            _targetMock = new Mock<IDamageable>();
            _strategy = new DirectAttackStrategy(_timeScaleServiceMock.Object);

            _targetMock.Setup(x => x.Damage(It.IsAny<IDamager>()))
                .Callback<IDamager>(x => _targetDamage += x.DamageAmount);
            _timeScaleServiceMock.Setup(x => x.TimeScale)
                .Returns(1);
            _damageAmount = 10;
            _regenerationTime = 2.5f;

            _targetDamage = 0;
            _strategy.Setup(_damageAmount, _targetMock.Object,
                _regenerationTime);
        }


        [Test]
        public void ShouldStartAttack()
        {
            _strategy.Start();
            _strategy.Update(_regenerationTime / 2);
            _strategy.Update(_regenerationTime / 2);

            Assert.AreEqual(_damageAmount * 2, _targetDamage);
        }


        [Test]
        public void ShouldStopAttack()
        {
            _strategy.Start();
            _strategy.Update(_regenerationTime / 2);
            _strategy.Stop();
            _strategy.Update(_regenerationTime / 2);

            Assert.AreEqual(_damageAmount, _targetDamage);
        }


        [Test]
        public void ShouldBeAffectedByTimeScale()
        {
            var timeScale = 3;
            _timeScaleServiceMock.Setup(x => x.TimeScale)
                .Returns(timeScale);
            _strategy.Start();
            _strategy.Update(_regenerationTime / 2 / timeScale);
            _strategy.Update(_regenerationTime / 2 / timeScale);

            Assert.AreEqual(_damageAmount * 2, _targetDamage);
        }
    }
}
