using Moq;
using NUnit.Framework;
using Splatrika.MobArenaMobile.Model;

namespace Splatrika.MobArenaMobile.UnitTests
{
    public class WaveTests
    {
        private Wave _wave;
        private Mock<MobSpawnPoint> _spawnPoint1Mock;
        private Mock<MobSpawnPoint> _spawnPoint2Mock;
        private Mock<ITimeScaleService> _timeScaleServiceMock;
        private Mock<IHealth> _mobMock;
        private bool _point1Called;
        private bool _point2Called;


        [SetUp]
        public void Init()
        {
            _spawnPoint1Mock = new Mock<MobSpawnPoint>(2);
            _spawnPoint2Mock = new Mock<MobSpawnPoint>(5);
            _mobMock = new Mock<IHealth>();
            _timeScaleServiceMock = new Mock<ITimeScaleService>();
            _point1Called = false;
            _point2Called = false;

            var points = new MobSpawnPoint[]
            {
                _spawnPoint1Mock.Object,
                _spawnPoint2Mock.Object
            };

            var configuration = new WaveConfiguration(points);

            _wave = new Wave(configuration, _timeScaleServiceMock.Object);

            _timeScaleServiceMock.SetupGet(x => x.TimeScale)
                .Returns(1);

            _spawnPoint1Mock.Setup(x => x.Spawn())
                .Callback(() => _point1Called = true)
                .Returns(_mobMock.Object);
            _spawnPoint2Mock.Setup(x => x.Spawn())
                .Callback(() => _point2Called = true)
                .Returns(_mobMock.Object);
        }


        [Test]
        public void ShouldBeUnactiveOnInit()
        {
            Assert.False(_wave.Active);
        }


        [Test]
        public void ShouldBeActiveOnStartCalled()
        {
            _wave.Start();

            Assert.True(_wave.Active);
        }


        [Test]
        public void ShouldCallSpawnPointInTime()
        {
            _wave.Start();
            _wave.Update(_spawnPoint1Mock.Object.SpawnTime / 2);
            _wave.Update(_spawnPoint1Mock.Object.SpawnTime / 2);
            Assert.True(_point1Called);

            var spawnTime = _spawnPoint2Mock.Object.SpawnTime
                - _spawnPoint1Mock.Object.SpawnTime;
            _wave.Update(spawnTime / 2);
            _wave.Update(spawnTime / 2);
            Assert.True(_point2Called);
        }


        [Test]
        public void ShouldCallMultipleSpawnPoints()
        {
            _wave.Start();
            _wave.Update(_spawnPoint2Mock.Object.SpawnTime);

            Assert.True(_point1Called);
            Assert.True(_point2Called);
        }


        [Test]
        public void ShouldNotCallSpawnPointsWhenUnactive()
        {
            _wave.Update(_spawnPoint2Mock.Object.SpawnTime);

            Assert.False(_point1Called);
            Assert.False(_point2Called);
        }


        [Test]
        public void ShouldBeFinishedOnAllMobsHasDied()
        {
            var finished = false;
            _wave.Finished += () => finished = true;
            _wave.Start();
            _wave.Update(_spawnPoint2Mock.Object.SpawnTime);

            _mobMock.Raise(x => x.Died += null);

            Assert.True(finished);
        }


        [Test]
        public void ShouldUseTimeScale()
        {
            var timeScale = 3;
            _timeScaleServiceMock.SetupGet(x => x.TimeScale)
                .Returns(timeScale);

            _wave.Start();
            _wave.Update(_spawnPoint1Mock.Object.SpawnTime / timeScale);

            Assert.True(_point1Called);
        }
    }
}
