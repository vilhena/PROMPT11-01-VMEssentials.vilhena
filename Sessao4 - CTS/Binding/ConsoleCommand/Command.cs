using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleCommand
{
    public class CommandAttribute: Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
    }

    public class CommandParameterAttribute: Attribute
    {
        public string Name { get; set; }
        public bool Required { get; set; }
        public string Description { get; set; }
    }

    public interface ICommand
    {
        void Run(TextWriter output);
    }

    public class MyFirstCommand : ICommand
    {
        public void Run(TextWriter output)
        {
            output.WriteLine("MyFirstCommand");
        }
    }

    [Command(Name = "list", Description = "Lists something")]
    public class ListCommand : ICommand
    {
        [CommandParameter(Name = "prm", Description = "a parameter")]
        public string _prm;

        public void Run(TextWriter output)
        {
            output.WriteLine(_prm);
        }
    }


}
