using Moq;
using NUnit.Framework;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.UnitTests
{
    public class WalkingMobTests
    {
        private WalkingMob _walkingMob;
        private Mock<IPlayerCharacter> _playerCharacterMock;
        private Mock<IFollowingPartial> _followingPartialMock;
        private Mock<IDamagablePartial> _damagablePartialMock;
        private Mock<ITimeScaleService> _timeScaleServiceMock;
        private WalkingMobConfiguration _configuration;


        [SetUp]
        public void Init()
        {
            _playerCharacterMock = new Mock<IPlayerCharacter>();
            _followingPartialMock = new Mock<IFollowingPartial>();
            _damagablePartialMock = new Mock<IDamagablePartial>();
            _timeScaleServiceMock = new Mock<ITimeScaleService>();

            _timeScaleServiceMock.SetupGet(x => x.TimeScale)
                .Returns(1);

            _walkingMob = new WalkingMob(
                _playerCharacterMock.Object,
                _followingPartialMock.Object,
                _damagablePartialMock.Object,
                _timeScaleServiceMock.Object);

            _configuration = new WalkingMobConfiguration(
                startPoint: new Vector3(1, 1, 2),
                speed: 15,
                atackDistance: 12,
                atackRegenerationTime: 2,
                atackDamage: 5,
                health: 10,
                walkingRegenerationTime: 1.5f,
                rewardPoints: 5);
        }


        [Test]
        public void ShouldNotBeActiveOnInit()
        {
            Assert.False(_walkingMob.Active);
        }


        [Test]
        public void ShouldBeStarted()
        {
            var started = false;
            _walkingMob.Started += () => started = true;
            _walkingMob.Start(_configuration);

            Assert.True(started);
        }


        [Test]
        public void ShouldNotMoveWhenUnactive()
        {
            var followingUpdated = false;
            _followingPartialMock.Setup(x => x.Update(It.IsAny<float>()))
                .Callback(() => followingUpdated = true);
            _walkingMob.Update(10);

            Assert.False(followingUpdated);
        }


        [Test]
        public void ShouldSetupFollowing()
        {
            var player = _playerCharacterMock.Object;
            var followingConfiguration = new FollowingConfiguration();
            _followingPartialMock
                .Setup(x => x.Start(It.IsAny<FollowingConfiguration>()))
                .Callback((FollowingConfiguration c) =>
                    followingConfiguration = c);
            _walkingMob.Start(_configuration);

            Assert.AreEqual(_configuration.StartPoint,
                followingConfiguration.Start);
            Assert.AreEqual(player.Position,
                followingConfiguration.Target);
            Assert.AreEqual(_configuration.AtackDistance,
                followingConfiguration.MinDistance);
            Assert.AreEqual(_configuration.Speed,
                followingConfiguration.Speed);
            Assert.AreEqual(_configuration.WalkingRegenerationTime,
                followingConfiguration.RegenerationTime);
        }


        [Test]
        public void ShouldSetupDamagable()
        {
            var friendBullet = new Mock<IFriendBullet>().Object;
            var anotherDamager = new Mock<IDamager>().Object;
            var damagableConfiguration = new DamagableConfiguration();
            _damagablePartialMock
                .Setup(x => x.Setup(It.IsAny<DamagableConfiguration>()))
                .Callback((DamagableConfiguration config) =>
                    damagableConfiguration = config);
            _walkingMob.Start(_configuration);

            Assert.AreEqual(_configuration.Health,
                damagableConfiguration.Health);
            Assert.True(damagableConfiguration
                .AllowedDamagers.Invoke(friendBullet));
            Assert.False(damagableConfiguration
                .AllowedDamagers.Invoke(anotherDamager));
        }


        [Test]
        public void ShouldBeDeactivatedOnDie()
        {
            _walkingMob.Start(_configuration);
            _damagablePartialMock.Raise(x => x.Died += null);

            Assert.False(_walkingMob.Active);
        }


        [Test]
        public void ShouldStartAtackingOnArriveThePlayer()
        {
            _walkingMob.Start(_configuration);

            IDamager playerDamager = null;
            _playerCharacterMock.Setup(x => x.Damage(It.IsAny<IDamager>()))
                .Callback((IDamager damager) => playerDamager = damager);
            _followingPartialMock.Raise(x => x.Arrived += null);
            _walkingMob.Update(_configuration.AtackRegenerationTime);

            Assert.AreEqual(_walkingMob, playerDamager);
        }


        [Test]
        public void ShouldRaiseMovementStarted()
        {
            bool movementStarted = false;
            _walkingMob.MovementStarted += () => movementStarted = true;
            _followingPartialMock.Raise(x => x.MovementStarted += null);

            Assert.True(movementStarted);
        }


        [Test]
        public void ShouldRaiseDirectionUpdated()
        {
            Vector3? updatedDirection = null;
            _walkingMob.DirectionUpdated += x => updatedDirection = x;
            Vector3 exceptedDirection = Vector3.left;
            _followingPartialMock.Raise(x => x.DirectionUpdated += null,
                exceptedDirection);

            Assert.NotNull(updatedDirection);
            Assert.AreEqual(exceptedDirection, updatedDirection);
        }


        [Test]
        public void ShouldRauseMovementStopped()
        {
            var movementStopped = false;
            _walkingMob.MovementStopped += () => movementStopped = true;
            _followingPartialMock.Raise(x => x.MovementStopped += null);

            Assert.True(movementStopped);
        }


        [Test]
        public void ShouldRaiseDied()
        {
            var died = false;
            _walkingMob.Died += () => died = true;
            _damagablePartialMock.Raise(x => x.Died += null);

            Assert.True(died);
        }


        [Test]
        public void ShouldRaiseHealthUpdated()
        {
            int? health = null;
            _walkingMob.HealthUpdated += x => health = x;
            var exceptedHealth = 3;
            _damagablePartialMock.Raise(x => x.HealthUpdated += null,
                exceptedHealth);

            Assert.NotNull(health);
            Assert.AreEqual(exceptedHealth, health);
        }


        [Test]
        public void ShouldStopMovementOnDamage()
        {
            _walkingMob.Start(_configuration);
            var hitted = false;
            _followingPartialMock.Setup(x => x.Hit())
                .Callback(() => hitted = true);
            _damagablePartialMock.Raise(x => x.Damaged += null);

            Assert.True(hitted);
        }
    }
}
