using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Contracts.View;
using HttpReflector.Views.Attributes;

namespace HttpReflector.Views
{
    [TemplateView("ExceptionView.txt")]
    public class ExceptionView : IView
    {
        public Exception Exception { get; set; }
    }
}
