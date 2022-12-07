using System;
using Moq;
using NUnit.Framework;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;

namespace Splatrika.MobArenaMobile.UnitTests
{
    public class NavigatedMovementTests
    {
        private NavigatedMovement _movement;
        private Mock<INavigationService> _navigationServiceMock;
        private Mock<ITimeScaleService> _timeScaleServiceMock;
        private float _speed;
        private Vector3 _target;

        private delegate void PathCallback(
            Vector3 from, Vector3 to, Vector3[] buffer, out int points);


        [SetUp]
        public void Init()
        {
            _navigationServiceMock = new Mock<INavigationService>();
            _timeScaleServiceMock = new Mock<ITimeScaleService>();
            var logger = new Mock<ILogger>().Object;
            var navigationConfiguration = new NavigationConfiguration(
                pathBuffersSize: 5);
            _movement = new NavigatedMovement(
                navigationConfiguration, _navigationServiceMock.Object,
                logger, _timeScaleServiceMock.Object);

            _timeScaleServiceMock.Setup(x => x.TimeScale)
                .Returns(1);
            _speed = 2f;
            _target = Vector3.right;
            _movement.Configure(speed: _speed, minDistance: 0);
        }


        [Test]
        public void ShouldStartMovement()
        {
            ConfigureNavigation(_movement.Position, _target, new Vector3[0]);
            var movementStarted = false;
            _movement.MovementStarted += () => movementStarted = true;
            _movement.SetDestination(_target);

            Assert.True(movementStarted);
        }


        [Test]
        public void ShouldMove()
        {
            var path = new Vector3[] { Vector3.zero, Vector3.right };
            var length = 1;
            ConfigureNavigation(_movement.Position, _target, path);
            _movement.SetDestination(_target);
            _movement.Update(length / _speed / 2);

            Assert.AreEqual(0.5f, _movement.Position.x, float.Epsilon);
            Assert.AreEqual(0f, _movement.Position.y, float.Epsilon);
            Assert.AreEqual(0f, _movement.Position.z, float.Epsilon);
        }


        [Test]
        public void ShouldContinueMovementAfterChangePosition()
        {
            var path1 = new Vector3[] { Vector3.zero, Vector3.up };
            var length1 = 1;
            var path2 = new Vector3[] { Vector3.down, Vector3.up };
            var length2 = 2;
            ConfigureNavigation(_movement.Position, _target, path1);
            ConfigureNavigation(Vector3.left, _target, path2);
            _movement.SetDestination(_target);
            _movement.Update(length1 / _speed / 2);
            _movement.SetPosition(Vector3.left);
            _movement.Update(length2 / _speed / 4);

            Assert.AreEqual(0f, _movement.Position.x, float.Epsilon);
            Assert.AreEqual(-0.5f, _movement.Position.y, float.Epsilon);
            Assert.AreEqual(0f, _movement.Position.z, float.Epsilon);
        }


        [Test]
        public void ShouldArrive()
        {
            var path = new Vector3[] { Vector3.zero, Vector3.right };
            var length = 1;
            ConfigureNavigation(_movement.Position, _target, path);
            var arrived = false;
            _movement.Arrived += () => arrived = true;
            _movement.SetDestination(_target);
            _movement.Update(length / _speed + 1);

            Assert.AreEqual(Vector3.right, _movement.Position);
            Assert.True(arrived);
        }


        [Test]
        public void ShouldArriveOnMinDistance()
        {
            _movement.Configure(speed: _speed, minDistance: 0.5f);
            var path = new Vector3[] { Vector3.zero, Vector3.right };
            var length = 1;
            ConfigureNavigation(_movement.Position, _target, path);
            var arrived = false;
            _movement.Arrived += () => arrived = true;
            _movement.SetDestination(_target);
            _movement.Update(length / _speed);

            Assert.AreEqual(0.5f, _movement.Position.x, float.Epsilon);
            Assert.AreEqual(0f, _movement.Position.y, float.Epsilon);
            Assert.AreEqual(0f, _movement.Position.z, float.Epsilon);
            Assert.True(arrived);
        }


        [Test]
        public void ShouldUpdateDirection()
        {
            var path = new Vector3[] {
                Vector3.zero, Vector3.right, Vector3.left, Vector3.zero };
            var lenght = 4;
            ConfigureNavigation(_movement.Position, _target, path);
            Vector3? updatedDirection = null;
            _movement.DirectionUpdated += x => updatedDirection = x;
            _movement.SetDestination(_target);
            _movement.Update(lenght / _speed / 2);

            Assert.NotNull(updatedDirection);
            Assert.AreEqual(Vector3.left, updatedDirection);
        }

        private void ConfigureNavigation(Vector3 from, Vector3 to,
            Vector3[] path)
        {
            var points = path.Length;

            _navigationServiceMock.Setup(
                x => x.TryGetPath(from, to, It.IsAny<Vector3[]>(), out points))
                .Callback(new PathCallback(
                    (Vector3 _, Vector3 __, Vector3[] buffer, out int p) =>
                {
                    p = points;
                    Array.Copy(path, buffer, points);
                }))
                .Returns(true);
        }
    }
}
