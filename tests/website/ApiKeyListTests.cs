using evefifo.model;
using evefifo.website;
using evefifo.website.controllers;
using evefifo.website.routing;
using evefifo.website.routing.responses;
using Microsoft.Owin.Testing;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.tests.website
{
    class ApiKeyListTests
    {
        private static TestServer GetServer(IRepository repo)
        {
            return TestServer.Create(Startup.Configuration(() => repo));
        }

        private static Mock<IRepository> CreateRepo()
        {
            var repo = new Mock<IRepository>();
            repo.SetupGet(x => x.ApiKeys).ReturnsAsync(new List<ApiKey> {
                new ApiKey { Id = 1234, Secret = "api_secret_1" }
            });
            return repo;
        }

        [Test]
        public async Task ApiKeyListContainsKeysInRepo()
        {
            var controller = new ApiKeyController();
            var response = await controller.List(new Request(HttpMethod.Get, "/apikeys", new Dictionary<string, string>(), CreateRepo().Object, null));
            var viewResponse = (ViewResponse)response;
            Assert.AreEqual("ApiKeyList", viewResponse.ViewName);
            Assert.AreEqual("api_secret_1", viewResponse.Model.ApiKeys[0].Secret);
        }

        [Test]
        public async Task ApiKeyListAddsKeysToRepo()
        {
            var controller = new ApiKeyController();
            var repo = CreateRepo();
            var postData = @"{ id: 1001, secret: ""a secret"" }";
            var postDataStream = new MemoryStream(Encoding.UTF8.GetBytes(postData));
            var response = await controller.Add(new Request(HttpMethod.Post, "/apikeys", new Dictionary<string, string>(), repo.Object, postDataStream));
            repo.Verify(x => x.AddApiKey(It.Is<ApiKey>(k => k.Id == 1001 && k.Secret == "a secret")));
            Assert.AreEqual("/apikeys", ((SeeOtherResponse)response).Location);
        }
    }
}