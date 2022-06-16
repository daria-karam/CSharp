using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab5Core.Archivation
{
    static class DirectoryWalker
    {
        public static List<FileInfo> WalkDirectoryTree(DirectoryInfo root)
        {
            List<FileInfo> files = null;

            try
            {
                files = root.GetFiles("*.*").ToList();
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            DirectoryInfo[] subDirs = root.GetDirectories();
            foreach (DirectoryInfo dirInfo in subDirs)
            {
                files.AddRange(WalkDirectoryTree(dirInfo));
            }

            return files;
        }
    }
}
