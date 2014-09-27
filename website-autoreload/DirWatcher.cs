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

        public DirWatcher(string path)
        {
            m_Watcher = new FileSystemWatcher();
            m_Watcher.Path = path;
            m_Watcher.Filter = "*.*";
            m_Watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime;
            m_Watcher.IncludeSubdirectories = true;
            m_Watcher.Changed += (obj, args) => Fire();
            m_Watcher.Created += (obj, args) => Fire();
            m_Watcher.Deleted += (obj, args) => Fire();
            m_Watcher.Renamed += (obj, args) => Fire();
            m_Watcher.EnableRaisingEvents = true;
        }

        private void Fire()
        {
            var handler = OnChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public void Dispose()
        {
            m_Watcher.Dispose();
        }

        public event EventHandler<EventArgs> OnChanged;
    }
}
