using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace website_autoreload
{
    class DirWatcher : IDisposable
    {
        private readonly FileSystemWatcher m_Watcher;
        private readonly string m_IgnorePath;

        public DirWatcher(string path, string ignorePath)
        {
            m_Watcher = new FileSystemWatcher();
            m_Watcher.Path = path;
            m_Watcher.Filter = "*.*";
            m_Watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime;
            m_Watcher.IncludeSubdirectories = true;
            m_Watcher.Changed += (obj, args) => Fire(args);
            m_Watcher.Created += (obj, args) => Fire(args);
            m_Watcher.Deleted += (obj, args) => Fire(args);
            m_Watcher.EnableRaisingEvents = true;
            m_IgnorePath = ignorePath;
        }

        private void Fire(FileSystemEventArgs e)
        {
            if (e.FullPath.Contains(m_IgnorePath)) return;
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType.ToString().ToLower());
            var handler = OnChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void Dispose()
        {
            m_Watcher.Dispose();
        }

        public event EventHandler<FileSystemEventArgs> OnChanged;
    }
}
