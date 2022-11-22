using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Splatrika.MobArenaMobile.Model;

namespace Splatrika.MobArenaMobile.UnitTests
{
    public class ScoreServiceTests
    {
        private ScoreService _scoreService;
        private Mock<IEnemy> _enemyMock;


        [SetUp]
        public void Init()
        {
            _enemyMock = new Mock<IEnemy>();
            var enemy1 = new Mock<IEnemy>().Object;
            var enemy2 = _enemyMock.Object;
            var enemies = new List<IEnemy>() { enemy1, enemy2 };

            _scoreService = new ScoreService(enemies);
        }


        [Test]
        public void ShouldAddScoreOnEnemyDied()
        {
            var reward = 10;
            float? updated = null;
            _scoreService.Updated += x => updated = x;
            KillEnemyWith(reward);
            
            Assert.AreEqual(reward, _scoreService.Score);
            Assert.NotNull(updated);
            Assert.AreEqual(reward, updated);
        }


        [Test]
        public void ShouldAddScoreOnOneEnemyDiedMultipleTimes()
        {
            var reward1 = 15;
            var reward2 = 5;
            var exceptedScore = reward1 + reward2;
            KillEnemyWith(reward1);
            KillEnemyWith(reward2);

            Assert.AreEqual(exceptedScore, _scoreService.Score);
        }


        private void KillEnemyWith(int reward)
        {
            _enemyMock.SetupGet(x => x.RewardPoints)
                 .Returns(reward);
            _enemyMock.Raise(x => x.Died += null, _enemyMock.Object);
        }
    }
}
