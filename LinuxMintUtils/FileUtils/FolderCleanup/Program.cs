using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using Microsoft.Extensions.Configuration;

namespace FolderCleanup
{
    internal static class Program
    {
        private static void Main()
        {
            var appSettings = GetAppConfigs();
            var errors = new List<string>();

            // call DeleteFiles() for each dir
            foreach (var dir in appSettings)
            {
                
            }

            // handle errors..
            if (errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }

                Console.ReadLine();
            }
            
        }

        private static List<string> DeleteFiles(string dir, int days)
        {
            var files = Directory.EnumerateFiles(dir, "*.*", SearchOption.AllDirectories);
            var notDeleted = new List<string>();
            
            foreach (var file in files)
            {
                var fi = new FileInfo(file);

                if (fi.CreationTime >= DateTime.Now.AddDays(days))
                {
                    continue;
                }

                try
                {
                    fi.Delete();
                }
                catch(IOException ex)
                {
                    notDeleted.Add($"File {fi.Name} can't be deleted: file is in use.");
                }
                catch(SecurityException ex)
                {
                    notDeleted.Add($"File {fi.Name} can't be deleted: insufficient permission.");
                }
                catch(UnauthorizedAccessException ex)
                {
                    notDeleted.Add($"File {fi.Name} can't be deleted: access denied.");
                }
            }

            return notDeleted;
        }
        
        private static IConfigurationSection GetAppConfigs()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            return configuration.GetSection("Settings");
        }
    }
}
