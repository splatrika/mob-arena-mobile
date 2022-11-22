using Moq;
using NUnit.Framework;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.UnitTests
{
    public class ShootingMobTests
    {
        private ShootingMobConfiguration _configuration;
        private ShootingMob _shootingMob;
        private Mock<IDamagablePartial> _damagablePartialMock;
        private Mock<IFollowingPartial> _followingPartialMock;
        private Mock<IBulletService> _bulletServiceMock;
        private Mock<ITimeScaleService> _timeScaleServiceMock;
        private Mock<IPlayerCharacter> _playerCharacterMock;
        private Mock<IPositionProvider> _shotPointMock;

        
        [SetUp]
        public void Init()
        {
            _damagablePartialMock = new Mock<IDamagablePartial>();
            _followingPartialMock = new Mock<IFollowingPartial>();
            _bulletServiceMock = new Mock<IBulletService>();
            _timeScaleServiceMock = new Mock<ITimeScaleService>();
            _playerCharacterMock = new Mock<IPlayerCharacter>();
            _shotPointMock = new Mock<IPositionProvider>();

            _shootingMob = new ShootingMob(
                _damagablePartialMock.Object,
                _followingPartialMock.Object,
                _bulletServiceMock.Object,
                _timeScaleServiceMock.Object,
                _playerCharacterMock.Object);

            _configuration = new ShootingMobConfiguration(
                health: 10,
                bulletsSpeed: 5.3f,
                bulletsDamage: 5,
                gunRegenerationTime: 3,
                start: new Vector3(2, 4, -8),
                movementSpeed: 12,
                movementRegenerationTime: 2,
                shootingPosition: new Vector3(2, 3, 2),
                shotPoint: _shotPointMock.Object,
                rewardPoints: 5);

            _playerCharacterMock.SetupGet(x => x.Position)
                .Returns(new Vector3(2, 33, -1));
            _playerCharacterMock.SetupGet(x => x.CenterOffset)
                .Returns(new Vector3(1, 1, -1));

            _timeScaleServiceMock.SetupGet(x => x.TimeScale)
                .Returns(1);
        }


        [Test]
        public void ShouldNotBeActiveOnInit()
        {
            Assert.False(_shootingMob.Active);
        }


        [Test]
        public void ShouldBeStarted()
        {
            var started = false;
            _shootingMob.Started += () => started = true;
            _shootingMob.Start(_configuration);

            Assert.True(started);
        }


        [Test]
        public void ShouldSetupFollowing()
        {
            var followingConfiguration = new FollowingConfiguration();
            _followingPartialMock
                .Setup(x => x.Start(It.IsAny<FollowingConfiguration>()))
                .Callback((FollowingConfiguration config) =>
                    followingConfiguration = config);
            _shootingMob.Start(_configuration);

            Assert.AreEqual(_configuration.Start,
                followingConfiguration.Start);
            Assert.AreEqual(_configuration.ShootingPosition,
                followingConfiguration.Target);
            Assert.AreEqual(0,
                followingConfiguration.MinDistance);
            Assert.AreEqual(_configuration.MovementSpeed,
                followingConfiguration.Speed);
            Assert.AreEqual(_configuration.MovementRegenerationTime,
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
            _shootingMob.Start(_configuration);

            Assert.AreEqual(_configuration.Health,
               damagableConfiguration.Health);
            Assert.True(damagableConfiguration
                .AllowedDamagers.Invoke(friendBullet));
            Assert.False(damagableConfiguration
                .AllowedDamagers(anotherDamager));
        }


        [Test]
        public void ShouldStartShootingOnArrive()
        {
            _shootingMob.Start(_configuration);
            var shootingStarted = false;
            _shootingMob.ShootingStarted += () => shootingStarted = true;
            _followingPartialMock.Raise(x => x.Arrived += null);

            Assert.True(shootingStarted);
        }


        [Test]
        public void ShouldShooting()
        {
            _shootingMob.Start(_configuration);
            var shootsCount = 0;
            var bulletConfiguration = new BulletConfiguration();
            _bulletServiceMock
                .Setup(x => x.Spawn(It.IsAny<BulletConfiguration>()))
                .Callback((BulletConfiguration config) =>
                    bulletConfiguration = config);
            _shotPointMock.SetupGet(x => x.Position)
                .Returns(new Vector3(1, 2.1f, -2));
            var exceptedShotPosition = _shootingMob.ShotPoint.Position;
            var exceptedTarget = _playerCharacterMock.Object.Position
                + _playerCharacterMock.Object.CenterOffset;
            var directionToPlayer = exceptedTarget - exceptedShotPosition;
            _shootingMob.Shot += () => shootsCount++;
            _followingPartialMock.Raise(x => x.Arrived += null);
            _shootingMob.Update(_configuration.GunRegenerationTime / 2);
            _shootingMob.Update(_configuration.GunRegenerationTime / 2);

            Assert.AreEqual(1,
                shootsCount);
            Assert.AreEqual(directionToPlayer,
                bulletConfiguration.Direction);
            Assert.AreEqual(_configuration.BulletsSpeed,
                bulletConfiguration.Speed);
            Assert.AreEqual(_configuration.BulletsDamage,
                bulletConfiguration.Damage);
            Assert.AreEqual(exceptedShotPosition,
                bulletConfiguration.Position);
        }


        [Test]
        public void ShouldReturnPosition()
        {
            _shootingMob.Start(_configuration);
            var exceptedPosition = new Vector3(2, -3.2f, 10.12f);
            _followingPartialMock.SetupGet(x => x.Position)
                .Returns(exceptedPosition);

            Assert.AreEqual(exceptedPosition, _shootingMob.Position);
        }


        [Test]
        public void ShouldRaiseDied()
        {
            _shootingMob.Start(_configuration);
            var died = false;
            _shootingMob.Died += () => died = true;
            _damagablePartialMock.Raise(x => x.Died += null);

            Assert.True(died);
        }


        [Test]
        public void ShouldRaiseHealthUpdated()
        {
            _shootingMob.Start(_configuration);
            int? updatedHealth = 0;
            _shootingMob.HealthUpdated += x => updatedHealth = x;
            var exceptedHealht = 10;
            _damagablePartialMock.Raise(x => x.HealthUpdated += null,
                exceptedHealht);

            Assert.NotNull(updatedHealth);
            Assert.AreEqual(exceptedHealht, updatedHealth);
        }


        [Test]
        public void ShouldRaiseMovementStarted()
        {
            _shootingMob.Start(_configuration);
            var movementStarted = false;
            _shootingMob.MovementStarted += () => movementStarted = true;
            _followingPartialMock.Raise(x => x.MovementStarted += null);

            Assert.True(movementStarted);
        }


        [Test]
        public void ShouldRaiseMovemenStopped()
        {
            _shootingMob.Start(_configuration);
            var movementStopped = false;
            _shootingMob.MovementStopped += () => movementStopped = true;
            _followingPartialMock.Raise(x => x.MovementStopped += null);

            Assert.True(movementStopped);
        }


        [Test]
        public void ShouldRaiseDirectionUpdated()
        {
            _shootingMob.Start(_configuration);
            Vector3? updatedDirection = null;
            _shootingMob.DirectionUpdated += x => updatedDirection = x;
            Vector3 exceptedDirection = Vector3.right;
            _followingPartialMock.Raise(x => x.DirectionUpdated += null,
                exceptedDirection);

            Assert.NotNull(updatedDirection);
            Assert.AreEqual(exceptedDirection, updatedDirection);
        }


        [Test]
        public void ShouldStopMovementOnDamage()
        {
            _shootingMob.Start(_configuration);
            var hitted = false;
            _followingPartialMock.Setup(x => x.Hit())
                .Callback(() => hitted = true);
            _damagablePartialMock.Raise(x => x.Damaged += null);

            Assert.True(hitted);
        }
    }
}
