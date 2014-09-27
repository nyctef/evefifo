using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace website_autoreload
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var runner = new OwinRunner())
            {
                runner.Start();

                using (var watcher = new DirWatcher("."))
                {
                    watcher.OnChanged += (o, a) =>
                    {
                        runner.Start();
                    };

                    Console.WriteLine("...");
                    Console.ReadLine();
                }
            }
        }
    }
}
