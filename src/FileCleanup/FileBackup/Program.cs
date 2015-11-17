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
        static void Main()
        {
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

                try
                {
                    Console.WriteLine("Backing up files in " + source + Environment.NewLine);
                    CopyFiles(source, target);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error copying files from" + source + " to " + target + Environment.NewLine);
                    Console.WriteLine(ex.ToString());
                    Console.ReadLine();
                    return;
                }
            }


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
                FileInfo sourceInfo = new FileInfo(sourceFile);

                if (sourceInfo.Name == "Thumbs.db")
                    continue;

                string destFile = Path.Combine(destinationPath, sourceInfo.Name);

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

            //recurse the directories
            string[] dirs = Directory.GetDirectories(sourcePath);
            foreach (string dir in dirs)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                CopyFiles(dir, Path.Combine(destinationPath, dirInfo.Name));
            }
        }


    }
}
