using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyReflect;
using System.IO;

namespace ReflectApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new DirectoryInfo(@"c:\program files");

            
            PID.Inspect(test);
            PID.InspectToFile(test);
            PID.Seek(test, "");

        }
    }
}
