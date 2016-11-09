using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace SiteBackup
{
    public class Program
    {
        public static string SourcePath = ConfigurationManager.AppSettings["SourcePath"];
        public static string BackupPath = ConfigurationManager.AppSettings["BackupPath"];

        public static string TargetPath;

        public static void Main(string[] args)
        {
            
            CreatBackup();
           
            Console.ReadKey();
        }
        public static void CreatBackup()
        {
            TargetPath=string.Format("{0}\\{1}",BackupPath,DateTime.Now.ToString("yyyyMMddHHmm"));
            var folders= ConfigurationManager.AppSettings["FoldersToBackUp"].Split('|');
            Console.Write("Target Created");
            Console.WriteLine(TargetPath);
            Directory.CreateDirectory(TargetPath);

            foreach (var f in folders)
            {
                Directory.CreateDirectory(string.Format("{0}\\{1}", TargetPath,f));
            }

            foreach (var f in folders)
            {
                CopyDir(string.Format("{0}\\{1}", SourcePath, f), string.Format("{0}\\{1}", TargetPath, f), false);
            }

            
            var wildcards= ConfigurationManager.AppSettings["RootWildcards"].Split('|');

            foreach (var w in wildcards)
            {
                Console.WriteLine("copying files with wildcard {0}", w);
                CopyFile(SourcePath, w, TargetPath);
            }

        }
        
        public static void CopyDir(string source, string target,bool deep=true)
        {
            Console.WriteLine("\nCopying Directory:\n  \"{0}\"\n-> \"{1}\"", source, target);

            if (!Directory.Exists(target)) Directory.CreateDirectory(target);
            string[] sysEntries = Directory.GetFileSystemEntries(source);

            foreach (string sysEntry in sysEntries)
            {
                string fileName = Path.GetFileName(sysEntry);
                string targetPath = Path.Combine(target, fileName);
                if (!deep && Directory.Exists(sysEntry))
                    continue;

                if (Directory.Exists(sysEntry))
                {
                    CopyDir(sysEntry, targetPath);
                    continue;
                }
               
                Console.WriteLine("\tCopying \"{0}\"", fileName);
                File.Copy(sysEntry, targetPath, true);
                
            }
           
        }
        public static void CopyFile(string sourcePath, string wildCard, string targetPath)
        {
            if (string.IsNullOrWhiteSpace(wildCard))
                return;

            DirectoryInfo di = new DirectoryInfo(sourcePath);

            FileInfo[] filelist = di.GetFiles(wildCard);

            foreach (FileInfo file in filelist)
            {
                File.Copy(string.Format("{0}\\{1}", file.DirectoryName, file.Name), string.Format("{0}\\{1}", targetPath, file.Name));   
            }
        }
    }


}
