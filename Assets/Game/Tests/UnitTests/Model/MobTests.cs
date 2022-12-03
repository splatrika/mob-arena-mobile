using Moq;
using NUnit.Framework;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.UnitTests
{
    public class MobTests
    {
        class TestMob : Mob<MobConfiguration>
        {
            public IFollowingStrategy StartedFollowingStrategy
                { get; private set; }
            public IAttackingStrategy StartedAttackingStrategy
                { get; private set; }
            public MobConfiguration Configuration { get; private set; }


            public TestMob(
                IFollowingStrategy followingStrategy,
                IAttackingStrategy attackingStrategy,
                ILogger logger)
                : base(followingStrategy, attackingStrategy, logger)
            {
            }


            protected override void OnStart(
                MobConfiguration configuration,
                IFollowingStrategy following,
                IAttackingStrategy attacking)
            {
                Configuration = configuration;
                StartedFollowingStrategy = following;
                StartedAttackingStrategy = attacking;
            }
        }


        private TestMob _mob;
        private Mock<IFollowingStrategy> _followingMock;
        private Mock<IAttackingStrategy> _attackingMock;
        private MobConfiguration _configuration;


        [SetUp]
        public void Init()
        {
            _followingMock = new Mock<IFollowingStrategy>();
            _attackingMock = new Mock<IAttackingStrategy>();
            _configuration = new MobConfiguration(
                health: 5,
                position: new Vector3(2, -4.2f),
                rewardPoints: 12);
            var logger = new Mock<ILogger>().Object;

            _mob = new TestMob(_followingMock.Object, _attackingMock.Object,
                logger);
        }


        [Test]
        public void ShouldStart()
        {
            var activated = false;
            _mob.Activated += () => activated = true;
            _mob.Start(_configuration);

            Assert.AreEqual(_configuration, _mob.Configuration);
            Assert.AreEqual(_attackingMock.Object,
                _mob.StartedAttackingStrategy);
            Assert.AreEqual(_followingMock.Object,
                _mob.StartedFollowingStrategy);
            Assert.True(_mob.Active);
            Assert.True(activated);
        }


        [Test]
        public void ShouldSetPositionOnStart()
        {
            Vector3? setted = null;
            _followingMock.Setup(x => x.SetPosition(It.IsAny<Vector3>()))
                .Callback<Vector3>(x => setted = x);
            _mob.Start(_configuration);

            Assert.NotNull(setted);
            Assert.AreEqual(_configuration.Position, setted);
        }


        [Test]
        public void ShouldStartAtackingOnArrived()
        {
            var started = false;
            _attackingMock.Setup(x => x.Start())
                .Callback(() => started = true);
            _mob.Start(_configuration);
            _followingMock.Raise(x => x.Arrived += null);

            Assert.True(started);
        }


        [Test]
        public void ShouldStopAttackingOnStartFollowing()
        {
            var stopped = false;
            _attackingMock.Setup(x => x.Stop())
                .Callback(() => stopped = true);
            _mob.Start(_configuration);
            _followingMock.Raise(x => x.StartedFollowing += null);

            Assert.True(stopped);
        }


        [Test]
        public void ShouldRaiseMovementStarted()
        {
            var raised = false;
            _mob.MovementStarted += () => raised = true;
            _mob.Start(_configuration);
            _followingMock.Raise(x => x.StartedFollowing += null);

            Assert.True(raised);
        }


        [Test]
        public void ShouldRaiseMovementStopped()
        {
            var raised = false;
            _mob.MovementStopped += () => raised = true;
            _mob.Start(_configuration);
            _followingMock.Raise(x => x.Arrived += null);

            Assert.True(raised);
        }


        [Test]
        public void ShouldRaiseDirectionUpdated()
        {
            Vector3? raised = null;
            _mob.DirectionUpdated += x => raised = x;
            _mob.Start(_configuration);
            var direction = new Vector3(3, -4);
            _followingMock.Raise(x => x.DirectionUpdated += null, direction);

            Assert.NotNull(raised);
            Assert.AreEqual(direction, raised);
        }
    }
}
