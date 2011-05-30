using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Contracts.Handler;
using HttpReflector.Contracts.View;
using HttpReflector.Controllers;
using HttpReflector.Controllers.Model;
using HttpReflector.Views;

namespace HttpReflector.Handlers
{
    public class RootHandler: IHandler
    {
        public IView Run()
        {
            return new RootView() { Contexts = AssemblyModel.ListContexts() };
        }
    }
}
