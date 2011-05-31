using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Contracts.Handler;
using HttpReflector.Contracts.View;
using HttpReflector.Controllers;
using HttpReflector.Handlers.Attributes;
using HttpReflector.Views;

namespace HttpReflector.Handlers
{
    public class ContextHandler:IHandler
    {
        [HandlerMapAttribute("{ctx}")]
        public string Context { get; set; }

        public IView Run()
        {
            var ctx = AssemblyModel.GetContext(Context);
            var namespaces = AssemblyModel.ListNamespaces(Context);

            var view = new ContextView
                           {
                               Context = ctx,
                               Assemblies = ctx.Assemblies.Values.ToList(),
                               Namespaces = namespaces
                           };

            return view;
        }
    }
}
