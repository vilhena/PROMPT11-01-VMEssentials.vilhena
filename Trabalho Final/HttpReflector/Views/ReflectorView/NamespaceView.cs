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
    [TemplateView("NamespaceView.txt")]
    public class NamespaceView: IView
    {
        public ReflectNamespace Namespace { get; set; }
        [CollectionView("NamespaceView.Type.txt")]
        public List<ReflectType> Types { get; set; }
    }
}
