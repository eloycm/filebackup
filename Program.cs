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
            Console.Write("hello world");
            if (args.Length > 0 && args[0] == "r")
                RevertBackup();
            else 
                CreatBackup();
           
            Console.ReadKey();
        }
        public static void CreatBackup()
        {
            TargetPath=string.Format("{0}\\{1}",BackupPath,DateTime.Now.ToString("yyyyMMddHHmm"));
            Console.Write("Target Created");
            Console.WriteLine(TargetPath);
            Directory.CreateDirectory(TargetPath);
            Directory.CreateDirectory(string.Format("{0}\\{1}", TargetPath, "bin"));
            Directory.CreateDirectory(string.Format("{0}\\{1}", TargetPath, "themes"));
            Directory.CreateDirectory(string.Format("{0}\\{1}", TargetPath, "layouts"));
            Directory.CreateDirectory(string.Format("{0}\\{1}", TargetPath, "App_Config"));

            CopyDir(string.Format("{0}\\{1}", SourcePath, "bin"), string.Format("{0}\\{1}", TargetPath, "bin"),false);
            CopyDir(string.Format("{0}\\{1}", SourcePath, "themes"), string.Format("{0}\\{1}", TargetPath, "themes"));
            CopyDir(string.Format("{0}\\{1}", SourcePath, "layouts"), string.Format("{0}\\{1}", TargetPath, "layouts"));
            CopyDir(string.Format("{0}\\{1}", SourcePath, "App_Config"), string.Format("{0}\\{1}", TargetPath, "App_Config"));
            File.Copy(string.Format("{0}\\{1}", SourcePath, "web.config"),string.Format("{0}\\{1}", TargetPath, "web.config"));
        }
        public static void RevertBackup()
        {
            string lastBackupPath = BackupPath.GetLastBackupPath();
            CopyDir(string.Format("{0}\\{1}", lastBackupPath, "bin"), string.Format("{0}\\{1}", SourcePath, "bin"), false);
            CopyDir(string.Format("{0}\\{1}", lastBackupPath, "themes"), string.Format("{0}\\{1}", SourcePath, "themes"));
            CopyDir(string.Format("{0}\\{1}", lastBackupPath, "layouts"), string.Format("{0}\\{1}", SourcePath, "layouts"));
            CopyDir(string.Format("{0}\\{1}", lastBackupPath, "App_Config"), string.Format("{0}\\{1}", SourcePath, "App_Config"));
            File.Copy(string.Format("{0}\\{1}", lastBackupPath, "web.config"), string.Format("{0}\\{1}", SourcePath, "web.config"));

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
    }


}
