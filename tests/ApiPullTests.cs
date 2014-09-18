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

            var oldCharacter = Character(apiKey: apiKey, sp: 6);
            var updatedCharacter = Character(apiKey: apiKey, sp: 7);

            var repo = new Mock<IRepository>();
            repo.Setup(x => x.Characters).ReturnsAsync(new List<evefifo.model.Character> { oldCharacter });
            repo.Setup(x => x.CharacterFromApi(apiKey, (int)oldCharacter.Id)).ReturnsAsync(updatedCharacter);

            ModelCharacter.UpdateExisting(repo.Object).Wait();

            repo.Verify(x => x.Replace(oldCharacter, updatedCharacter));
        }

        private static evefifo.model.Character Character(string name=null, evefifo.model.ApiKey apiKey=null, int? id=null, int? sp=null)
        {
            var _apiKey = new evefifo.model.ApiKey { Id = 1234, Secret = "asdf" };
            return new evefifo.model.Character
            {
                Name = name ?? "Foo",
                ApiKey = apiKey ?? _apiKey,
                Id = id ?? 3,
                SP = sp ?? 5,
            };
        }
    }
}
