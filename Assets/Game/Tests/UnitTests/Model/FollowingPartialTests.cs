using System;
using Moq;
using NUnit.Framework;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.UnitTests
{
    public class FollowingPartialTests
    {
        private FollowingPartial _followingPartial;
        private FollowingConfiguration _configuration;
        private Mock<INavigationPartial> _navigationPartialMock;
        private Mock<ITimeScaleService> _timeScaleServiceMock;


        [SetUp]
        public void Init()
        {
            _navigationPartialMock = new Mock<INavigationPartial>();
            _timeScaleServiceMock = new Mock<ITimeScaleService>();
            _followingPartial = new FollowingPartial(
                _navigationPartialMock.Object, _timeScaleServiceMock.Object);

            _timeScaleServiceMock.SetupGet(x => x.TimeScale)
                .Returns(1);

            _configuration = new FollowingConfiguration(
                startPoint: new Vector3(10, -11),
                followTarget: new Vector3(-100, -12),
                minDistance: 3,
                speed: 2,
                regenerationTime: 14);

            _navigationPartialMock
               .Setup(x => x.Start(It.IsAny<Vector3>(), It.IsAny<Vector3>(),
                   It.IsAny<float>()))
               .Returns(true);
        }


        [Test]
        public void ShouldStartMovement()
        {
            Vector3? start = null;
            Vector3? target = null;
            float? speed = null;
            SetupNavigationMock(
                x => start = x,
                x => target = x,
                x => speed = x);
            _followingPartial.Start(_configuration);

            Assert.AreEqual(_configuration.Start, start);
            Assert.AreEqual(_configuration.Target, target);
            Assert.AreEqual(_configuration.Speed, speed);
            Assert.True(_followingPartial.IsMoving);
        }


        [Test]
        public void ShouldStopMovementOnHit()
        {
            _followingPartial.Start(_configuration);
            var stopped = false;
            _navigationPartialMock.Setup(x => x.Stop())
                .Callback(() => stopped = true);
            _followingPartial.Hit();

            Assert.True(stopped);
            Assert.False(_followingPartial.IsMoving);
        }


        [Test]
        public void ShouldContinueMovementAfterTime()
        {
            _followingPartial.Start(_configuration);
            var lastPosition = new Vector3(1, -2.5f);
            _navigationPartialMock.SetupGet(x => x.Position)
                .Returns(lastPosition);
            _followingPartial.Hit();

            Vector3? start = null;
            Vector3? target = null;
            float? speed = null;
            SetupNavigationMock(
                x => start = x,
                x => target = x,
                x => speed = x);

            var movementStarted = false;
            _followingPartial.MovementStarted += () => movementStarted = true;
            _followingPartial.Update(_configuration.RegenerationTime / 2);
            _followingPartial.Update(_configuration.RegenerationTime / 2);

            Assert.AreEqual(lastPosition, start);
            Assert.AreEqual(_configuration.Target, target);
            Assert.AreEqual(_configuration.Speed, speed);
            Assert.True(_followingPartial.IsMoving);
            Assert.True(movementStarted);
        }


        [Test]
        public void ShouldStopMovementOnDistance()
        {
            var stopped = false;
            var arrived = false;
            _followingPartial.Start(_configuration);
            _followingPartial.Arrived += () => arrived = true;
            var exceptedPosition = _configuration.Target
                + (Vector3.left * _configuration.MinDistance);
            var position = _configuration.Target
                + (Vector3.left * (_configuration.MinDistance / 2));
            _navigationPartialMock.SetupGet(x => x.Position)
                .Returns(position);
            _navigationPartialMock.Setup(x => x.Stop())
                .Callback(() => stopped = true);
            _followingPartial.Update(0);

            Assert.True(stopped);
            Assert.True(arrived);
            Assert.AreEqual(exceptedPosition, _followingPartial.Position);
            Assert.False(_followingPartial.IsMoving);
        }


        [Test]
        public void ShouldRaiseArrived()
        {
            _configuration.MinDistance = 0;

            var arrived = false;
            _followingPartial.Arrived += () => arrived = true;
            _followingPartial.Start(_configuration);
            _navigationPartialMock.SetupGet(x => x.Position)
                .Returns(_configuration.Target);
            _followingPartial.Update(0);

            Assert.True(arrived);
        }


        [Test]
        public void ShouldUseTimeScale()
        {
            var timeScale = 2;
            _timeScaleServiceMock.SetupGet(x => x.TimeScale)
                .Returns(timeScale);
            var movementStarted = false;
            _navigationPartialMock
                .Setup(x => x.Start(It.IsAny<Vector3>(), It.IsAny<Vector3>(),
                    It.IsAny<float>()))
                .Callback(() => movementStarted = true);
            _followingPartial.Start(_configuration);
            _followingPartial.Hit();
            _followingPartial.Update(
                _configuration.RegenerationTime / timeScale);

            Assert.True(movementStarted);
        }


        private void SetupNavigationMock(Action<Vector3> startSetter,
            Action<Vector3> targetSetter, Action<float> speedSetter)
        {
            _navigationPartialMock
                .Setup(x => x.Start(It.IsAny<Vector3>(), It.IsAny<Vector3>(),
                    It.IsAny<float>()))
                .Callback((Vector3 start, Vector3 target, float speed) =>
                {
                    startSetter.Invoke(start);
                    targetSetter.Invoke(target);
                    speedSetter.Invoke(speed);
                })
                .Returns(true);
        }
    }
}
