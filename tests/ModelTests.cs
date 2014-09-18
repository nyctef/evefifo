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
        public class CanSaveAndRetrieve
        {
            [Test]
            public void Character()
            {
                using (var db = new EvefifoContext())
                {
                    db.Characters.RemoveRange(db.Characters);

                    var character = new Character
                    {
                        Name = "The Mittani",
                        CloneName = "Alpha",
                        CloneSP = 900000,
                        SP = 12345678,
                        CorpName = "Goonfleet",
                        SecStatus = -9.9
                    };

                    db.Characters.Add(character);
                    db.SaveChanges();
                }

                using (var db = new EvefifoContext())
                {
                    var character = db.Characters.Single();
                    Assert.AreEqual("The Mittani", character.Name);
                }
            }
        }

    }
}
