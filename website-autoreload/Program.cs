using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace website_autoreload
{
    class Program
    {
        static void Main(string[] args)
        {
            var autoreloadPath = "_autoreload";
            var lastUpdate = DateTime.Now;
            using (var runner = new OwinRunner(autoreloadPath))
            {
                using (var dirCopy = new LiveDirCopy(new DirectoryInfo("."), autoreloadPath))
                using (var watcher = new DirWatcher(".", autoreloadPath))
                {
                    runner.Start();

                    Observable
                        .FromEventPattern(watcher, "OnChanged")
                        .Select(PathFromEventArgs)
                        .Buffer(EveryHalfSecond())
                        .Where(SomethingHasHappened)
                        .Select(paths => paths.Distinct())
                        .Subscribe(paths =>
                        {
                            runner.Stop();
                            foreach (var path in paths)
                            {
                                dirCopy.Update(path);
                            }
                            runner.Start();
                        });

                    Console.WriteLine("...");
                    Console.ReadLine();
                }
            }
        }

        private static string PathFromEventArgs(EventPattern<object> a)
        {
            return Path.GetFullPath(((FileSystemEventArgs)a.EventArgs).FullPath);
        }

        private static TimeSpan EveryHalfSecond()
        {
            return TimeSpan.FromMilliseconds(500);
        }

        private static bool SomethingHasHappened(IEnumerable<string> things)
        {
            return things.Any();
        }
    }
}
