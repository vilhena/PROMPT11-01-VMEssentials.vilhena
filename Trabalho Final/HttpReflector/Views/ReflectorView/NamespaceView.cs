using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Contracts.View;
using HttpReflector.Controllers.Model;

namespace HttpReflector.Views
{
    public class NamespaceView: IView
    {
        public ReflectNamespace Namespace { get; set; }
        public List<ReflectType> Types { get; set; }
        public List<ReflectNamespace> Namespaces { get; set; }
    }
}
