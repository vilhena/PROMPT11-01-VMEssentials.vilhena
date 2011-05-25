using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleCommand
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandRepository.Instance.With<ListCommand>().With<MyFirstCommand>().Run(args);
        }
    }
}
