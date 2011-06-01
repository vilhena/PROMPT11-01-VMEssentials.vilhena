using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Contracts.View;
using HttpReflector.Controllers.Model;

namespace HttpReflector.Views
{
    public class ContextNamespaceView: IView
    {
        public ReflectContext Context { get; set; }
        public List<ReflectNamespace> Namespaces { get; set; }
    }
}
