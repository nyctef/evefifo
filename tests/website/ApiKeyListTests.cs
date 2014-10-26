﻿using evefifo.model;
using evefifo.website;
using evefifo.website.controllers;
using Microsoft.Owin.Testing;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            using (var server = GetServer(CreateRepo().Object))
            {
                var body = await Utils.GetBody(await server.HttpClient.GetAsync("/apikeys"));
                StringAssert.Contains("1234", body.InnerText);
                StringAssert.Contains("api_secret_1", body.InnerText);
            }
        }

        [Test]
        public async Task ApiKeyListAddsKeysToRepo()
        {
            var controller = new ApiKeyController();
            var environment = new Dictionary<string, object>();
            var repo = CreateRepo();
            var postData = @"{ id: 1001, secret: ""a secret"" }";
            environment["owin.RequestBody"] = new MemoryStream(Encoding.UTF8.GetBytes(postData));
            environment["owin.RequestPath"] = "/apikeys";
            environment["owin.ResponseHeaders"] = new Dictionary<string, string[]>();
            environment["evefifo.Repository"] = repo.Object;
            await controller.Add(environment);
            repo.Verify(x => x.AddApiKey(It.Is<ApiKey>(k => k.Id == 1001 && k.Secret == "a secret")));
        }
    }
}