using evefifo.model;
using eZet.EveLib.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.api_pull
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new EvefifoContext())
            {
                var repo = new Repository(db);
                if (args.Length > 2)
                {
                    var charId = args[0];
                    var apiKey = args[1];
                    var apiSecret = args[2];

                    AddNewCharacter(repo, Int32.Parse(apiKey), apiSecret, Int32.Parse(charId)).Wait();
                }
                else
                {
                    ModelCharacter.UpdateExisting(repo).Wait();
                }
            }
        }

        static async Task AddNewCharacter(IRepository repo, int apiKey, string apiSecret, int charId)
        {
            var character = await repo.CharacterFromApi(new model.ApiKey { Id = apiKey, Secret = apiSecret }, charId);

            using (var db = new EvefifoContext())
            {
                db.Characters.Add(character);
                await db.SaveChangesAsync();
            }
        }
    }
}
