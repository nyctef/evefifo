using evefifo.website;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using System.Net;
using Moq;
using evefifo.model;

namespace evefifo.tests.website
{
    public class RoutingTests
    {
        private static TestServer GetServer()
        {
            var repo = new Mock<IRepository>();
            var bob = new Character {
                Name = "Bob",
                Notifications = new List<Notification>(),
                SkillQueue = new SkillQueue(new List<SkillQueue.Entry> { new SkillQueue.Entry(123, "a skill", 3, 2, 3, DateTime.Now, DateTime.Now) })
            };
            repo.Setup(x => x.Characters).ReturnsAsync(new List<Character> {
                bob
            });
            repo.Setup(x => x.Character(1234)).ReturnsAsync(bob);
            return TestServer.Create(Startup.Configuration(() => repo.Object));
        }

        [Test]
        public async void RootPathReturns200()
        {
            using (var server = GetServer())
            {
                var response = await server.HttpClient.GetAsync("/");
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Test]
        public async void RootPathReturnsCharacterList()
        {
            using (var server = GetServer())
            {
                var response = await server.HttpClient.GetAsync("/");
                var title = await Utils.GetTitle(response);
                StringAssert.Contains("Character List", title);
            }
        }

        [Test]
        public async void CharacterPathReturnsCharacterDetails()
        {
            using (var server = GetServer())
            {
                var response = await server.HttpClient.GetAsync("/character/1234");
                var title = await Utils.GetTitle(response);
                StringAssert.Contains("Bob", title);
            }
        }

        [Test]
        public async void UnknownPathReturns404()
        {
            using (var server = TestServer.Create<Startup>())
            {
                var response = await server.HttpClient.GetAsync("/asldkjfhalskdjfhlasdjfhas");
                Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Test]
        public async void ApiKeysPathReturnsApiKeys()
        {
            using (var server = TestServer.Create<Startup>())
            {
                var response = await server.HttpClient.GetAsync("/apikey");
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }
    }
}
