using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyReflect;
using System.IO;

namespace ReflectApp
{

    class teste
    {
        public int inteiro { get; set; }
        public List<string> lista { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var test = new DirectoryInfo(@"c:\program files");

            
            PID.Inspect(test);
            PID.InspectToFile(test);
            //PID.Seek(test, "");

            PID.Seek(test.GetDirectories(), "");
            teste tt = new teste();
            tt.lista = new List<string>();
            tt.lista.Add("teasd");
            PID.Seek(tt, "");

        }
    }
}
