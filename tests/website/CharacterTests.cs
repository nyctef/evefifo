﻿using evefifo.model;
using evefifo.website.models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.tests.website
{
    public class CharacterTests
    {
        public class CharacterModelTests
        {
            [Test]
            public void TotalQueueLength()
            {
                var now = DateTime.Now;
                var model = new CharacterModel(new Character
                {
                    SkillQueue = new SkillQueue(new List<SkillQueue.Entry>
                    {
                        new SkillQueue.Entry(1, "skill", 2, 3, 4, now, now.AddHours(25).AddMonths(2))
                    })
                }, now);

                Assert.AreEqual("62 days, 1 hour", model.TotalQueueLength);
            }

            [Test]
            public void TotalQueueLengthTakesTZIntoAccount()
            {
                var now = DateTime.Now;
                var skillEnd = now.ToUniversalTime().AddMinutes(5);

                var model = new CharacterModel(new Character
                {
                    SkillQueue = new SkillQueue(new List<SkillQueue.Entry>
                    {
                        new SkillQueue.Entry(1, "skill", 2, 3, 4, now, skillEnd)
                    })
                }, now);

                Assert.AreEqual("5 minutes", model.TotalQueueLength);
            }
        }
    }
}
