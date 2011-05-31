using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Contracts.Handler;
using HttpReflector.Contracts.View;
using HttpReflector.Controllers;
using HttpReflector.Controllers.Model;
using HttpReflector.Handlers.Attributes;
using HttpReflector.Views;

namespace HttpReflector.Handlers
{
    public class NamespaceHandler:IHandler
    {
        [HandlerMapAttribute("{ctx}")]
        public string Context { get; set; }

        [HandlerMapAttribute("{namespacePrefix}")]
        public string Namespace { get; set; }

        public IView Run()
        {
            var ctx = AssemblyModel.GetNamespace(Context, Namespace);

            var typesList = ctx.Types.Values.Select(lazyType => lazyType.Value).ToList();

            var view = new NamespaceView
                           {
                               Namespace = ctx,
                               Types = typesList
                           };

            return view;
        }
    }
}
