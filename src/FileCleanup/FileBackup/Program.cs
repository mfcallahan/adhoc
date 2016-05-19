using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileBackup
{
    class Program
    {
        static List<string> noSource = new List<string>();
        static List<string> notCopied = new List<string>();

        private static string[] ignoreFiles =
        {
            "Thumbs.db",
            "gitconfig",            
            ".gitignore",
            ".gitattributes"
        };

        private static string[] ignoreDirs =
        {
            ".git",
            "_nocopy"
        };

        static void Main()
        {
            Console.WriteLine("FileBackup.exe begin.");
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "FileBackupSettings.txt"))
            {
                Console.WriteLine(@"FileCleanup.exe error: ""FileBackupSettings.txt"" not found.");
                Console.ReadLine();
                return;
            }

            var folders = GetSettings();           

            foreach (var f in folders)
            {
                if (f.StartsWith("//", StringComparison.CurrentCulture) || string.IsNullOrWhiteSpace(f))
                    continue;

                var source = f.Split(',')[0].Trim();
                var target = f.Split(',')[1].Trim();

                if(!Directory.Exists(source))
                {
                    noSource.Add(source);
                    continue;
                }

                if (!Directory.Exists(target))
                {
                    Directory.CreateDirectory(target);
                }

                Console.WriteLine("Backing up files from: " + source);
                CopyFiles(source, target);
            }

            if (noSource.Count > 0)
            {
                Console.WriteLine("WARNING - Could not copy from these locations, folder does not exist:");
                foreach (string f in noSource)
                    Console.WriteLine(f);
            }

            if (notCopied.Count > 0)
            {
                Console.WriteLine("WARNING - Could not copy these files:");
                foreach (string f in notCopied)
                    Console.WriteLine(f);
            }

            Console.WriteLine("FileBackup.exe complete.");
            Console.ReadLine();

        }//end main

        static string[] GetSettings()
        {
            return File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "FileBackupSettings.txt");
        }

        static void CopyFiles(string sourcePath, string destinationPath)
        {           

            //bool dirExisted = DirExists(destinationPath);
            //create destination directory if not exist
            if (!Directory.Exists(destinationPath))
                Directory.CreateDirectory(destinationPath);

            //get the source files
            string[] srcFiles = Directory.GetFiles(sourcePath);

            foreach (string sourceFile in srcFiles)
            {
                foreach (string f in ignoreFiles)
                {
                    if (sourceFile.Contains(f))
                        goto Skip;
                }

                FileInfo sourceInfo = new FileInfo(sourceFile);
                string destFile = Path.Combine(destinationPath, sourceInfo.Name);

                try
                {
                    if (File.Exists(destFile))
                    {
                        FileInfo destInfo = new FileInfo(destFile);
                        if (sourceInfo.LastWriteTime > destInfo.LastWriteTime)
                        {
                            //file is newer, so copy it
                            File.Copy(sourceFile, Path.Combine(destinationPath, sourceInfo.Name), true);
                        }
                    }
                    else
                    {
                        File.Copy(sourceFile, Path.Combine(destinationPath, sourceInfo.Name));
                    }
                }
                catch (Exception ex)
                {
                    notCopied.Add(sourceInfo.FullName + " can't be copied: " + ex.Message);
                }

                Skip:;

            }

            //recurse the directories
            string[] dirs = Directory.GetDirectories(sourcePath);
            foreach (string dir in dirs)
            {
                foreach (string f in ignoreDirs)
                {
                    if (dir.Contains(f))
                        goto Skip;
                }

                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                CopyFiles(dir, Path.Combine(destinationPath, dirInfo.Name));
                Skip:;
            }
        }


    }
}
