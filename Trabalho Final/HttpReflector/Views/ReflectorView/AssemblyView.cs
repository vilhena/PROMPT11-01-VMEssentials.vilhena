using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Contracts.View;
using HttpReflector.Controllers.Model;

namespace HttpReflector.Views
{
    public class AssemblyNamespaceView : IView
    {
        public ReflectNamespace Namespace { get; set; }
        public List<ReflectType> Types { get; set; }
    }
    public class AssemblyView: IView
    {
        public ReflectAssembly Assembly { get; set; }
        public List<AssemblyNamespaceView> NamespacesWithTypes { get; set; }
    }
}
