using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SiteBackup
{
    public static class extensions
    {
        public static string GetLastBackupPath(this string path)
        {
            var rs = new DirectoryInfo(path).GetDirectories()
                .ToList().FindAll(w=>w.Name.IsNumeric())
                       .OrderByDescending(d => d.CreationTimeUtc).First();

            return rs.FullName;
        }
        public static bool IsNumeric(this string s)
        {
            bool rs = Regex.IsMatch(s, @"^\d+$");
            return rs;

        }
    }
}
