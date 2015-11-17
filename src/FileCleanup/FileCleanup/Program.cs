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

                try
                {                   
                    Console.WriteLine("Cleaning up files in folder " + folder + Environment.NewLine + "older than " + Math.Abs(days) + " day(s)");
                    DeleteFiles(folder, days);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(@"Error deleting files: " + folder + Environment.NewLine);
                    Console.WriteLine(ex.ToString());
                    Console.ReadLine();
                    return;
                }               
            }

        }//end main

        static string[] GetSettings()
        {
            return File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "FileCleanupSettings.txt");
        }

        static void DeleteFiles(string folder, int days)
        {
            var files = Directory.EnumerateFiles(folder, "*.*", SearchOption.AllDirectories);

            foreach (string f in files)
            {
                FileInfo fi = new FileInfo(f);
                if (fi.CreationTime < DateTime.Now.AddDays(days))      
                    fi.Delete();                
            }

            var folders = Directory.EnumerateDirectories(folder);

            foreach (string f in folders)
            {
                DirectoryInfo di = new DirectoryInfo(f);
                if (di.GetFiles().Length == 0 && !di.Name.Contains("Complete"))
                    di.Delete(true);
            }

        }
    }
}
