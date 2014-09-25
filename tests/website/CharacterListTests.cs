using evefifo.model;
using evefifo.website;
using Microsoft.Owin.Testing;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.tests.website
{
    class CharacterListTests
    {
        private static TestServer GetServer(IRepository repo)
        {
            return TestServer.Create(Startup.Configuration(context => repo));
        }

        [Test]
        public async Task CharacterListContainsCharactersInRepo()
        {
            var repo = new Mock<IRepository>();
            var char1 = new Character { Name = "char1", Notifications = new List<Notification> { } };
            var char2 = new Character { Name = "char2", Notifications = new List<Notification> { null, null } };
            repo.Setup(x => x.Characters).ReturnsAsync(new List<Character>
            {
                char1, char2
            });

            using (var server = GetServer(repo.Object))
            {
                var body = await Utils.GetBody(await server.HttpClient.GetAsync("/"));
                StringAssert.Contains("char1 (0)", body.InnerText);
                StringAssert.Contains("char2 (2)", body.InnerText);
            }
        }
    }
}
