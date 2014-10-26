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
using System.Net.Http;
using evefifo.tests;

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
            repo.SetupGet(x => x.ApiKeys).ReturnsAsync(new List<ApiKey> {
                new ApiKey { Id = 1234, Secret = "api_secret_1" }
            });
            return TestServer.Create(Startup.Configuration(() => repo.Object));
        }

        private static async Task<HttpResponseMessage> ResourceAtPath(string path)
        {
            using (var server = GetServer())
            {
                return await server.HttpClient.GetAsync(path);
            }
        }

        private static async Task<HttpResponseMessage> PostToPath(string path, HttpContent content)
        {
            using (var server = GetServer())
            {
                return await server.HttpClient.PostAsync(path, content);
            }
        }

        [Test]
        public async void RootPathReturns200()
        {
            await ResourceAtPath("/").HasStatusCode(HttpStatusCode.OK);
        }

        [Test]
        public async void RootPathReturnsCharacterList()
        {
            await ResourceAtPath("/").HasTitle("evefifo - Character List");
        }

        [Test]
        public async void CharacterPathReturnsCharacterDetails()
        {
            await ResourceAtPath("/characters/1234").HasTitle("evefifo - Bob");
        }

        [Test]
        public async void NonExistentCharacterPathReturns404()
        {
            await ResourceAtPath("/characters/9999").HasStatusCode(HttpStatusCode.NotFound);
        }

        [Test]
        public async void UnknownPathReturns404()
        {
            await ResourceAtPath("/asldkjfhalskdjfhlasdjfhas").HasStatusCode(HttpStatusCode.NotFound);
        }

        [Test]
        public async void ApiKeysPathReturnsApiKeys()
        {
            var response = await ResourceAtPath("/apikeys");
            response.HasStatusCode(HttpStatusCode.OK);
            response.HasTitle("evefifo - Api keys");
        }

        [Test]
        public async void ApiKeysPOSTReturnsApiKeys()
        {
            var response = await PostToPath("/apikeys", new StringContent("{id:1,secret:\"a\"}"));
            response.HasStatusCode(HttpStatusCode.SeeOther);
            response.HasHeader("Location", "/apikeys");
        }
    }
}
