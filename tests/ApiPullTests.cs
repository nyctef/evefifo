using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using evefifo.api_pull;

namespace tests
{
    public class ApiPullTests
    {
        [Test]
        public void ReplacesOldCharacterDataWithDataFromApi()
        {
            var apiKey = new evefifo.model.ApiKey { Id = 1234, Secret = "asdf" };
            var oldCharacter = new evefifo.model.Character
            {
                Name = "Foo",
                ApiKey = apiKey,
                Id = 3,
                SP = 5,
            };

            var updatedCharacter = new evefifo.model.Character
            {
                Name = "Foo",
                ApiKey = apiKey,
                Id = 3,
                SP = 6,
            };

            var repo = new Mock<IRepository>();
            repo.Setup(x => x.Characters).ReturnsAsync(new List<evefifo.model.Character> { oldCharacter });
            repo.Setup(x => x.CharacterFromApi(apiKey, (int)oldCharacter.Id)).ReturnsAsync(updatedCharacter);

            ModelCharacter.UpdateExisting(repo.Object).Wait();

            repo.Verify(x => x.Replace(oldCharacter, updatedCharacter));
        }
    }
}
