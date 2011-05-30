using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Contracts.Handler;
using HttpReflector.Contracts.View;
using HttpReflector.Controllers;
using HttpReflector.Views;

namespace HttpReflector.Handlers
{
    public class ContextHandler:IHandler
    {
        public string Context { get; set; }

        public IView Run()
        {
            var ctx = AssemblyModel.GetContext(Context);

            var view = new ContextView
                           {
                               Context = ctx,
                               Assemblies = ctx.Assemblies.Values.ToList(),
                               Namespaces = ctx.Namespaces.Values.ToList()
                           };


            return view;
        }
    }
}
