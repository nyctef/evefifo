using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace website_autoreload
{
    public static class Utils
    {
        /// <summary>
        /// This is still the official way to copy a directory.
        /// http://msdn.microsoft.com/en-us/library/bb762914%28v=vs.110%29.aspx
        /// (modified to take *Info classes instead of strings)
        /// </summary>
        public static void CopyDirectory(DirectoryInfo sourceDir, DirectoryInfo destDir, bool copySubDirs=true)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo[] dirs = sourceDir.GetDirectories();

            if (!sourceDir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDir.FullName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!destDir.Exists)
            {
                destDir.Create();
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = sourceDir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDir.FullName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDir.FullName, subdir.Name);
                    CopyDirectory(subdir, new DirectoryInfo(temppath), copySubDirs);
                }
            }
        }
    }
}
