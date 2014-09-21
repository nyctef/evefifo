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
                    var apiKeyId = args[1];
                    var apiSecret = args[2];
                    var apiKey = new model.ApiKey { Id = Int32.Parse(apiKeyId), Secret = apiSecret };
                    ModelCharacter.AddNew(repo, apiKey, Int32.Parse(charId)).Wait();
                }
                else
                {
                    ModelCharacter.UpdateExisting(repo).Wait();
                }
                db.SaveChanges();
            }
        }
    }
}
