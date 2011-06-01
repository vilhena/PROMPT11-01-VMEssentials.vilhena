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
    public class ConstructorHandler: IHandler
    {
        [HandlerMapAttribute("{ctx}")]
        public string Context { get; set; }

        [HandlerMapAttribute("{namespace}")]
        public string Namespace { get; set; }

        [HandlerMapAttribute("{shortName}")]
        public string TypeName { get; set; }

        public IView Run()
        {
            var type = AssemblyModel.GetCtsType(Context, Namespace, TypeName);

            return new ConstructorView
                       {
                           Type = type,
                           Constructors = type.Contructors
                       };
        }
    }
}
