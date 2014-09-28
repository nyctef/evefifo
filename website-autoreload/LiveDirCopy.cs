using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace website_autoreload
{
    class LiveDirCopy : IDisposable
    {
        private readonly DirectoryInfo m_SourceDir;
        private readonly DirectoryInfo m_TargetDir;

        public LiveDirCopy(DirectoryInfo sourceDir, string subDir)
        {
            m_SourceDir = sourceDir;
            m_TargetDir = new DirectoryInfo(Path.Combine(sourceDir.FullName, subDir));
            DeleteIfExists(m_TargetDir);
            m_TargetDir.Create();
            Utils.CopyDirectory(sourceDir, m_TargetDir);
        }

        public void Update(string path)
        {
            if (!(Path.IsPathRooted(path))) throw new ArgumentException("path must be rooted", "path");
            if (!path.StartsWith(m_SourceDir.FullName)) throw new ArgumentException("path must be below m_SourceDir", "path");

            var relativePath = path.Substring(m_SourceDir.FullName.Length).Trim('\\');
            var target = Path.Combine(m_TargetDir.FullName, relativePath);
            if (Directory.Exists(path))
            {
                Console.WriteLine("Copying dir "+path+" to "+target);
                DeleteIfExists(new DirectoryInfo(target));
                Directory.CreateDirectory(target);
            }
            else if (File.Exists(path))
            {
                Console.WriteLine("Copying file "+path+" to "+target);
                DeleteIfExists(new FileInfo(target));
                File.Copy(path, target);
            }
            else
            {
                throw new ArgumentException("path does not exist", "path");
            }
        }

        private static void DeleteIfExists(FileInfo file)
        {
            try
            {
                file.Delete();
            }
            catch (FileNotFoundException)
            { 
            }
        }
        
        private static void DeleteIfExists(DirectoryInfo dir)
        {
            try
            {
                dir.Delete(true);
            }
            catch (DirectoryNotFoundException)
            { 
            }
        }

        public void Dispose()
        {
            try
            {
                m_TargetDir.Delete(true);
            }
            catch (Exception)
            {
            }
        }
    }
}
