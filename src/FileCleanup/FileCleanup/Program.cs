using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCleanup
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("FileCleanup.exe begin.");
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "FileCleanupSettings.txt"))
            {
                Console.WriteLine(@"FileCleanup.exe error: ""FileCleanupSettings.txt"" not found.");
                Console.ReadLine();
                return;
            }

            var folders = GetSettings();

            foreach (var f in folders)
            {
                if (f.StartsWith("//", StringComparison.CurrentCulture) || string.IsNullOrWhiteSpace(f))
                    continue;

                var folder = f.Split(',')[0].Trim();
                var days = -Math.Abs(int.Parse(f.Split(',')[1].Trim()));

                 
                Console.WriteLine("Cleaning up files in folder " + folder + Environment.NewLine + "older than " + Math.Abs(days) + " day(s)");
                DeleteFiles(folder, days);



                Console.ReadLine();
                return;
            
            }

        }//end main

        static string[] GetSettings()
        {
            return File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "FileCleanupSettings.txt");
        }

        static List<string> DeleteFiles(string folder, int days)
        {
            var files = Directory.EnumerateFiles(folder, "*.*", SearchOption.AllDirectories);
            List<string> notDeleted = new List<string>();

            foreach (string f in files)
            {
                FileInfo fi = new FileInfo(f);
                if (fi.CreationTime < DateTime.Now.AddDays(days))
                {
                    try
                    {
                        fi.Delete();

                    }
                    catch(Exception ex)
                    {
                        notDeleted.Add("File: " + fi.Name + " can't be deleted: " + ex.Message);
                    }                   
                }
                                   
            }

            var folders = Directory.EnumerateDirectories(folder);

            foreach (string f in folders)
            {
                DirectoryInfo di = new DirectoryInfo(f);
                if (di.GetFiles().Length == 0 && !di.Name.Contains("Complete"))
                {
                    try
                    {
                        di.Delete(true);

                    }
                    catch (Exception ex)
                    {
                        notDeleted.Add("Folder: " + di.Name + " can't be deleted: " + ex.Message);
                    }
                }                    
            }

            return notDeleted;
        }

    }
}
