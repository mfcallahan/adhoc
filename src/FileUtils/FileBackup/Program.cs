using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileBackup
{
    /*   
     *   FileBackup.exe will copy all newer files and folders from/to the specified source and 
     *   destination folders in App.config.  FileBackup.exe is run as a Windows scheduled task every night.
     */


    class Program
    {
        static List<string> noSource = new List<string>();
        static List<string> notCopied = new List<string>();

        static List<string> ignoreFolderPaths = new List<string>();
        static List<string> ignoreFolderNames = new List<string>();
        static List<string> ignoreExts = new List<string>();
        static List<string> ignoreFiles = new List<string>();       

        static void Main()
        {
            Console.WriteLine("FileBackup.exe begin.");
            Console.WriteLine();

            var folders = ConfigurationManager.AppSettings.AllKeys
                            .Where(key => key.StartsWith("backupFolder", StringComparison.Ordinal))
                            .Select(key => ConfigurationManager.AppSettings[key])
                            .ToList();

            ignoreFolderPaths = ConfigurationManager.AppSettings["ignoreFolderNames"].Split(',').ToList();
            ignoreFolderNames = ConfigurationManager.AppSettings["ignoreFolderNames"].Split(',').ToList();
            ignoreExts = ConfigurationManager.AppSettings["ignoreExts"].Split(',').ToList();
            ignoreFiles = ConfigurationManager.AppSettings["ignoreFiles"].Split(',').ToList();

            foreach (var f in folders)
            {               
                var source = f.Split(',')[0].Trim();
                var target = f.Split(',')[1].Trim();                

                if (!Directory.Exists(source))
                {
                    noSource.Add(source);
                    continue;
                }

                Console.WriteLine("Backing up files from folder: " + source + Environment.NewLine + "To folder: " + target + Environment.NewLine);
                CopyFiles(source, target);
            }

            bool alert = false;

            if (noSource.Count > 0)
            {
                alert = true;
                Console.WriteLine("WARNING - Could not copy from these locations, folder does not exist:");
                foreach (string f in noSource)
                    Console.WriteLine(f);
            }

            if (notCopied.Count > 0)
            {
                alert = true;
                Console.WriteLine("WARNING - Could not copy these files:");
                foreach (string f in notCopied)
                    Console.WriteLine(f);
            }

            if (alert)
                Console.ReadLine();

            return;

        }//end main


        static void CopyFiles(string sourcePath, string destinationPath)
        {           
            //create destination directory if not exist
            //if (!Directory.Exists(destinationPath))
            //    Directory.CreateDirectory(destinationPath);

            //get the source files
            string[] srcFiles = Directory.GetFiles(sourcePath);

            foreach (string sourceFile in srcFiles)
            {
                foreach (string f in ignoreFiles)
                {
                    if (sourceFile.ToLower().Contains(f.ToLower()))
                        goto Skip;
                }

                foreach (string e in ignoreExts)
                {
                    if (Path.GetExtension(sourceFile).ToLower() == e.ToLower())
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
                foreach (string f in ignoreFolderPaths)
                {
                    if (dir.TrimEnd(Path.DirectorySeparatorChar).ToLower() == f.TrimEnd(Path.DirectorySeparatorChar).ToLower())
                        goto Skip;
                }

                foreach (string f in ignoreFolderNames)
                {
                    if (dir.ToLower().Contains(f.ToLower()))
                        goto Skip;
                }

                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                CopyFiles(dir, Path.Combine(destinationPath, dirInfo.Name));
                Skip:;
            }
        }
    }
}
