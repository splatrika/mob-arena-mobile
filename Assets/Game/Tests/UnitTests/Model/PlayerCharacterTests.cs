using System;
using Moq;
using NUnit.Framework;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.UnitTests
{
    public class PlayerCharacterTests
    {
        public const float ShootRegenerationTime = 10;
        public const int Health = 10;

        private PlayerCharacter _playerCharacter;
        private PlayerCharacterConfiguration _configuration;
        private DamagableConfiguration _damagableConfiguration;
        private Mock<IFriendBulletService> _friendBulletServiceMock;
        private Mock<ITimeScaleService> _timeScaleServiceMock;
        private Mock<IDamagablePartial> _damagablePartialMock;


        [SetUp]
        public void Init()
        {
            _friendBulletServiceMock = new Mock<IFriendBulletService>();
            _timeScaleServiceMock = new Mock<ITimeScaleService>();
            _damagablePartialMock = new Mock<IDamagablePartial>();

            _timeScaleServiceMock.SetupGet(x => x.TimeScale)
                .Returns(1);

            _damagablePartialMock
                .Setup(x => x.Setup(It.IsAny<DamagableConfiguration>()))
                .Callback((DamagableConfiguration configuration) =>
                    _damagableConfiguration = configuration);

            _configuration = new PlayerCharacterConfiguration(
                health: Health,
                shootRegenerationTime: ShootRegenerationTime,
                position: new Vector3(1, 12),
                direction: Vector3.left,
                centerOffset: new Vector3(2, -3.4f));

            _playerCharacter = new PlayerCharacter(
                _configuration,
                _friendBulletServiceMock.Object,
                _timeScaleServiceMock.Object,
                new Mock<ILogger>().Object,
                _damagablePartialMock.Object);
        }


        [Test]
        public void ShouldSetupDamagable()
        {
            var anotherDamager = new Mock<IDamager>().Object;
            var friendBullet = new Mock<IFriendBullet>().Object;

            Assert.AreEqual(_configuration.Health,
                _damagableConfiguration.Health);
            Assert.True(_damagableConfiguration
                .AllowedDamagers.Invoke(anotherDamager));
            Assert.False(_damagableConfiguration
                .AllowedDamagers.Invoke(friendBullet));
        }


        [Test]
        public void ShouldNotTakeDamageByFriendBullet()
        {
            var friendBulletMock = new Mock<IFriendBullet>();
            friendBulletMock.SetupGet(x => x.DamageAmount)
                .Returns(10);
            var lastHealth = _playerCharacter.Health;
            var healthUpdated = false;
            _playerCharacter.HealthUpdated += _ => healthUpdated = true;
            _playerCharacter.Damage(friendBulletMock.Object);

            Assert.AreEqual(lastHealth, _playerCharacter.Health);
            Assert.False(healthUpdated);
        }


        [Test]
        public void ShouldRotate()
        {
            var nonNormalizedDirection = new Vector2(10, 15);
            var normalizedDirection = nonNormalizedDirection.normalized;
            var exceptedDirection = new Vector3(
                normalizedDirection.x, 0, normalizedDirection.y);
            Vector3? directionUpdated = null;
            _playerCharacter.Rotated += direction =>
                directionUpdated = direction;
            _playerCharacter.SetDirection(nonNormalizedDirection);

            Assert.AreEqual(exceptedDirection, _playerCharacter.Direction);
            Assert.NotNull(directionUpdated);
            Assert.AreEqual(exceptedDirection, directionUpdated);
        }


        [Test]
        public void ShouldStartShooting()
        {
            Vector3? shootDirection = null;
            Vector3? shootPosition = null;

            _friendBulletServiceMock
                .Setup(x => x.Spawn(It.IsAny<Vector3>(), It.IsAny<Vector3>()))
                .Callback<Vector3, Vector3>((position, direction) =>
                {
                    shootDirection = direction;
                    shootPosition = position;
                });

            var direction = new Vector3(10, 3).normalized;
            _playerCharacter.SetDirection(direction);
            _playerCharacter.StartShooting();
            _playerCharacter.Update(ShootRegenerationTime);

            Assert.NotNull(shootDirection);
            Assert.NotNull(shootPosition);
            Assert.AreEqual(_playerCharacter.Direction, shootDirection);
            Assert.AreEqual(_playerCharacter.Position, shootPosition);
        }


        [Test]
        public void ShouldStopShooting()
        {
            var shootCount = 0;
            _friendBulletServiceMock
                .Setup(x => x.Spawn(It.IsAny<Vector3>(), It.IsAny<Vector3>()))
                .Callback(() => shootCount++);

            _playerCharacter.StartShooting();
            _playerCharacter.Update(ShootRegenerationTime);
            _playerCharacter.StopShooting();
            _playerCharacter.Update(ShootRegenerationTime);

            Assert.AreEqual(1, shootCount);
        }


        [Test]
        public void ShouldRegenerateWhileNotShooting()
        {
            var shootCount = 0;
            _friendBulletServiceMock
                .Setup(x => x.Spawn(It.IsAny<Vector3>(), It.IsAny<Vector3>()))
                .Callback(() => shootCount++);

            _playerCharacter.StartShooting();
            _playerCharacter.Update(ShootRegenerationTime);
            _playerCharacter.StopShooting();
            _playerCharacter.Update(ShootRegenerationTime);
            _playerCharacter.StartShooting();
            _playerCharacter.Update(0);

            Assert.AreEqual(2, shootCount);
        }


        [Test]
        public void TimeScaleShouldBeAppliedOnShootRegeneration()
        {
            var timeScale = 2;
            var shootCount = 0;
            _friendBulletServiceMock
                .Setup(x => x.Spawn(It.IsAny<Vector3>(), It.IsAny<Vector3>()))
                .Callback(() => shootCount++);
            _timeScaleServiceMock.SetupGet(x => x.TimeScale)
                .Returns(timeScale);

            _playerCharacter.StartShooting();
            _playerCharacter.Update(ShootRegenerationTime / timeScale);

            Assert.AreEqual(1, shootCount);
        }


        [Test]
        public void ShouldRaiseDied()
        {
            var died = false;
            _playerCharacter.Died += () => died = true;
            _damagablePartialMock.Raise(x => x.Died += null);

            Assert.True(died);
        }


        [Test]
        public void ShouldRaiseHealthUpdated()
        {
            int? health = null;
            _playerCharacter.HealthUpdated += x => health = x;
            int exceptedHealth = 23;
            _damagablePartialMock.Raise(x => x.HealthUpdated += null,
                exceptedHealth);

            Assert.NotNull(health);
            Assert.AreEqual(exceptedHealth, health);
        }
    }
}
