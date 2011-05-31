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
    [TemplateView("ContextNamespaceView.txt")]
    public class ContextNamespaceView: IView
    {
        public ReflectContext Context { get; set; }
        [CollectionView("ContextView.Namespaces.txt")]
        public List<ReflectNamespace> Namespaces { get; set; }
    }
}
