using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleCommand
{
    class CommandRepository
    {
        private IDictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();

        private static CommandRepository _instance;

        private CommandRepository() { }

        public static CommandRepository Instance
        {
            get { return _instance ?? (_instance = new CommandRepository()); }
        }


        public CommandRepository With<T>() where T:ICommand
        {
            Type tType = typeof (T);
            if (tType.GetCustomAttributesData() != null && tType.GetCustomAttributesData().Count > 0
                && tType.GetCustomAttributesData().First() != null)
            {
                foreach (var args in tType.GetCustomAttributesData().First().NamedArguments)
                {
                    if (args.MemberInfo.Name == "Name")
                        this._commands.Add(args.TypedValue.Value.ToString(), (ICommand) Activator.CreateInstance(tType));
                }
            }


            return this;
        }

        public void Run(string [] args)
        {
            var queue = new Queue<string>(args);
            string commandName = queue.Dequeue();

            ICommand command = _commands[commandName];


            //Errado tem que existir um mapeamento usando os Attributes
            string prop = "_" + queue.Dequeue().Trim('-');
            string value = queue.Dequeue();

            command.GetType().GetField(prop).SetValue(command, value);


            command.Run(Console.Out);
            
        }
    }
}
