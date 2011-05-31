using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Contracts.View;
using HttpReflector.Controllers.Model;
using HttpReflector.Views.Attributes;

namespace HttpReflector.Views
{
    //TODO: use by default same name of class plus html
    [TemplateView("AssemblyView.txt")]
    public class AssemblyView: IView
    {
        public ReflectAssembly Assembly { get; set; }
        [CollectionView("AssemblyView.Namespaces.txt")]
        public List<ReflectNamespace> Namespaces { get; set; }
    }
}
