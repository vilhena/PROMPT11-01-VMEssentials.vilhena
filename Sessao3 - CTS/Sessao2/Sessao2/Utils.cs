using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sessao2
{
    class Utils
    {
        public static void ProcessFiles(DirectoryInfo rootFolder, Func<FileInfo, bool> pred, Action<FileInfo> action)
        {
            foreach (var file in rootFolder.GetFiles())
            {
                if (pred(file))
                {
                    action(file);
                }
            }
        }
    }
}
