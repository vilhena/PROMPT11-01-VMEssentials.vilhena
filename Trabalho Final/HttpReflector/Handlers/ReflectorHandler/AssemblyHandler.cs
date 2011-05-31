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
    public class AssemblyHandler:IHandler
    {
        [HandlerMapAttribute("{ctx}")]
        public string Context { get; set; }

        [HandlerMapAttribute("{assemblyName}")]
        public string Assembly { get; set; }

        public IView Run()
        {
            var ctx = AssemblyModel.GetAssembly(Context, Assembly);

            var view = new AssemblyView
                           {
                               Assembly = ctx,
                               //Namespaces = ctx.Namespaces.GetAllChildrenValues(new List<string>()).ToList()
                           };

            return view;
        }
    }
}
