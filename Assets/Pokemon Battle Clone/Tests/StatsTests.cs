using NUnit.Framework;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;

namespace Pokemon_Battle_Clone.Tests
{
    public class StatsTests
    {
        [Test]
        public void CalculateLevel1Stats()
        {
            var baseStats = new StatSet(30, 56, 35, 25, 35, 72);
            var ivs = new StatSet(8, 22, 24, 25, 25, 2);
            var statsData = new StatsData(1, baseStats, Nature.Adamant(), new StatSet(), ivs);

            Assert.That(statsData.Stats, Is.EqualTo(new StatSet(11, 6, 5, 4, 5, 6)));
        }
        
        [Test]
        public void CalculateLevel50Stats()
        {
            var baseStats = new StatSet(30, 56, 35, 25, 35, 72);
            var ivs = new StatSet(8, 22, 24, 25, 25, 2);
            var statsData = new StatsData(50, baseStats, Nature.Adamant(), new StatSet(), ivs);

            Assert.That(statsData.Stats, Is.EqualTo(new StatSet(94, 79, 52, 37, 52, 78)));
        }
        
        [Test]
        public void CalculateLevel100Stats()
        {
            var baseStats = new StatSet(30, 56, 35, 25, 35, 72);
            var ivs = new StatSet(8, 22, 24, 25, 25, 2);
            var statsData = new StatsData(100, baseStats, Nature.Adamant(), new StatSet(), ivs);

            Assert.That(statsData.Stats, Is.EqualTo(new StatSet(178, 152, 99, 72, 100, 151)));
        }

        [Test]
        public void StatsModifier_EmptyCase()
        {
            var statsModifier = new StatsModifier();
            
            Assert.That(statsModifier.GetMultiplier(Stat.Attack), Is.EqualTo(1f));
            Assert.That(statsModifier.GetStage(Stat.Attack), Is.EqualTo(0));
        }
    }
}