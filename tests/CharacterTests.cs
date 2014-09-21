using evefifo.model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tests
{
    [TestFixture]
    public class CharacterTests
    {
        private static List<T> List<T>(params T[] elements) { return new List<T>(elements); }

        [Test]
        public void SkillQueueWithEntriesIsNotEmpty()
        {
            var queue = new SkillQueue(List(new SkillQueue.Entry(1, "two", 3, 4, 5, DateTime.Now, DateTime.Now)));
            Assert.False(queue.IsEmpty());
        }

        [Test]
        public void SkillQueueWithNoEntriesIsEmpty()
        {
            var queue = new SkillQueue(List<SkillQueue.Entry>());
            Assert.True(queue.IsEmpty());
        }

        [Test]
        public void SkillQueueWhichEndsBeforeOneDayFromNowHasSpace()
        {
            var entries = List(
                new SkillQueue.Entry(1, "two", 3, 4, 5, DateTime.Now.AddHours(-1), DateTime.Now.AddHours(+1))
                );

            Assert.True(new SkillQueue(entries).HasSpace());
        } 

        [Test]
        public void SkillQueueWhichEndsAfterOneDayFromNowDoesNotHaveSpace()
        {
            var entries = List(
                new SkillQueue.Entry(1, "two", 3, 4, 5, DateTime.Now.AddHours(-1), DateTime.Now.AddHours(+1)),
                new SkillQueue.Entry(2, "three", 4, 5, 6, DateTime.Now.AddHours(1), DateTime.Now.AddDays(60))
                );

            Assert.False(new SkillQueue(entries).HasSpace());
        } 
    }
}
