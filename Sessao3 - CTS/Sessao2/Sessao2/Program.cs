using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sessao2
{
    class Program
    {
        public static DateTime date = new DateTime(2011, 05, 10);
        public static int size = 10000;

        public static bool IsFileToList(FileInfo file)
        {
            return (file.LastWriteTime > date) || (file.Length > size);
        }

        public static void ListFile(FileInfo file)
        {
            Console.WriteLine("pathname={0} data={1} dimensão={2}",file.FullName, file.LastWriteTime, file.Length);
        }


        static void Main(string[] args)
        {
            //Func<FileInfo,bool> predDelegate = new ();
            
            Utils.ProcessFiles(new DirectoryInfo(@"c:\windows"), IsFileToList, ListFile);
        }
    }
}
