using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Contracts.Handler;
using HttpReflector.Contracts.View;
using HttpReflector.Controllers;

namespace HttpReflector.Handlers
{
    public class ContextAssemblyHandler:IHandler
    {
        public string Context { get; set; }

        public IView Run()
        {

            AssemblyModel.ListContextAssemblies(Context);

            return null;
        }
    }
}
