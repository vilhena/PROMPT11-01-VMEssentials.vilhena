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
    public class AssemblyHandler:IHandler
    {
        [HandlerMapAttribute("{ctx}")]
        public string Context { get; set; }

        [HandlerMapAttribute("{assemblyName}")]
        public string Assembly { get; set; }

        public IView Run()
        {
            var ctx = AssemblyModel.GetAssembly(Context, Assembly);
            var nsWithTypes = new List<AssemblyNamespaceView>();
            foreach(var ns in ctx.Context.Namespaces.GetAllChildrenValues(new List<string>()))
            {
                if (ns.Types.Count != 0)
                {
                    var typeList = new List<ReflectType>();
                    foreach (var type in ns.Types.Values)
                    {
                        typeList.Add(type.Value);
                    }
                    nsWithTypes.Add(new AssemblyNamespaceView()
                                        {
                                            Namespace = ns,
                                            Types = typeList
                                        });
                }
            }
            

            var view = new AssemblyView
                           {
                               Assembly = ctx,
                               NamespacesWithTypes = nsWithTypes
                           };

            return view;
        }
    }
}
