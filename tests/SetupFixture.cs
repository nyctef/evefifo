using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.tests
{

#if !NCRUNCH

    [SetUpFixture]
    public class SetupFixture
    {
        [SetUp]
        public void SetUp()
        {
            // Recreate an empty sqlite database for the tests to run against

            AppDomain.CurrentDomain.SetData("DataDirectory", @".\");

            const string db = "evefifo.sqlite";
            if (File.Exists(db))
            {
                File.Delete(db);
            }
            SQLiteConnection.CreateFile(db);
            using (var conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings["EvefifoContext"].ConnectionString))
            using (var command = conn.CreateCommand())
            {
                conn.Open();
                command.CommandText = File.ReadAllText(@"..\..\..\model\scripts\create-sqlite-db.sql");
                command.ExecuteNonQuery();
            }
        }
    }

#endif

}
