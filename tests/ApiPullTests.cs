using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using evefifo.api_pull;
using evefifo.model;

namespace tests
{
    public class ApiPullTests
    {
        [Test]
        public void ReplacesOldCharacterDataWithDataFromApi()
        {
            var apiKey = new ApiKey { Id = 1234, Secret = "asdf" };

            var oldCharacter = Character(apiKey: apiKey, sp: 6);
            var updatedCharacter = Character(apiKey: apiKey, sp: 7);

            var repo = new Mock<IRepository>();
            repo.Setup(x => x.Characters).ReturnsAsync(new List<Character> { oldCharacter });
            repo.Setup(x => x.CharacterFromApi(apiKey, (int)oldCharacter.Id)).ReturnsAsync(updatedCharacter);

            ModelCharacter.UpdateExisting(repo.Object).Wait();

            repo.Verify(x => x.Replace(oldCharacter, updatedCharacter));
        }

        [Test]
        public void CanAddNewCharacter()
        {
            var apiKey = new ApiKey { Id = 1234, Secret = "asdf" };
            const int charId = 3;
            var character = Character(apiKey: apiKey, id: charId);

            var repo = new Mock<IRepository>();
            repo.Setup(x => x.CharacterFromApi(apiKey, charId)).ReturnsAsync(character);

            ModelCharacter.AddNew(repo.Object, apiKey, charId).Wait();

            repo.Verify(x => x.AddCharacter(character));
        }

        private static Character Character(string name=null, ApiKey apiKey = null, int? id=null, int? sp=null)
        {
            return new Character
            {
                Name = name ?? "Foo",
                ApiKey = apiKey ?? new ApiKey { Id = 1234, Secret = "asdf" },
                Id = id ?? 3,
                SP = sp ?? 5,
            };
        }
    }
}
