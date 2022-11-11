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
        private Mock<IFriendBulletService> _friendBulletServiceMock;
        private Mock<ITimeScaleService> _timeScaleServiceMock;


        [SetUp]
        public void Init()
        {
            _friendBulletServiceMock = new Mock<IFriendBulletService>();
            _timeScaleServiceMock = new Mock<ITimeScaleService>();

            _timeScaleServiceMock.SetupGet(x => x.TimeScale)
                .Returns(1);

            var configuration = new PlayerCharacterConfiguration(
                health: Health,
                shootRegenerationTime: ShootRegenerationTime,
                position: new Vector3(1, 12),
                direction: Vector3.left);

            _playerCharacter = new PlayerCharacter(
                configuration,
                _friendBulletServiceMock.Object,
                _timeScaleServiceMock.Object,
                new Mock<ILogger>().Object);
        }

        
        [Test]
        public void ShouldTakeDamage()
        {
            var damageAmount = _playerCharacter.Health / 2;
            var bulletMock = new Mock<IDamager>();
            bulletMock.SetupGet(x => x.DamageAmount)
                .Returns(damageAmount);
            var exceptedHealth = _playerCharacter.Health - damageAmount;
            int? updatedHealth = null;
            _playerCharacter.HealthUpdated += health => updatedHealth = health;
            _playerCharacter.Damage(bulletMock.Object);

            Assert.AreEqual(exceptedHealth, _playerCharacter.Health);
            Assert.NotNull(updatedHealth);
            Assert.AreEqual(exceptedHealth, updatedHealth);
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
        public void ShouldDie()
        {
            var bulletMock = new Mock<IDamager>();
            bulletMock.SetupGet(x => x.DamageAmount)
                .Returns(_playerCharacter.Health);
            var died = false;
            _playerCharacter.Died += () => died = true;
            _playerCharacter.Damage(bulletMock.Object);

            Assert.True(died);
            Assert.True(_playerCharacter.IsDied);
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
    }
}
