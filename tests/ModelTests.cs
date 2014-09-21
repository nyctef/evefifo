using evefifo.model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tests
{
    public class ModelTests
    {
        [Category("Integration")]
        public class CanSaveAndRetrieve
        {
            [TestFixtureSetUp]
            public void TestFixtureSetUp()
            {
                using (var db = new EvefifoContext())
                {
                    db.ApiKeys.RemoveRange(db.ApiKeys);
                    db.Characters.RemoveRange(db.Characters);
                    db.SaveChanges();
                }
            }

            [Test]
            public void Character()
            {
                using (var db = new EvefifoContext())
                {
                    var character = new Character
                    {
                        Name = "The Mittani",
                        CloneName = "Alpha",
                        CloneSP = 900000,
                        SP = 12345678,
                        CorpName = "Goonfleet",
                        SecStatus = -9.9,
                        ApiKey = new ApiKey { Id = 1234, Secret = "asdf" },
                        SkillQueue = new SkillQueue(new List<SkillQueue.Entry> { new SkillQueue.Entry(1, "two", 3, 4, 5, DateTime.Now, DateTime.Now) }),
                    };

                    db.Characters.Add(character);
                    db.SaveChanges();
                }

                using (var db = new EvefifoContext())
                {
                    var character = db.Characters.Single();
                    Assert.AreEqual("The Mittani", character.Name);
                    Assert.AreEqual(1234, character.ApiKey.Id);
                    Assert.AreEqual(1, character.SkillQueue.Entries.Count);
                }
            }
        }

    }
}
