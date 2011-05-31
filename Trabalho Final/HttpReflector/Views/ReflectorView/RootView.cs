using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Contracts.View;
using HttpReflector.Controllers.Model;
using HttpReflector.Views.Attributes;

namespace HttpReflector.Views
{
    [TemplateView("RootView.txt")]
    public class RootView:IView
    {
        [CollectionView("RootView.Context.txt")]
        public List<ReflectContext> Contexts { get; set; }
    }
}
