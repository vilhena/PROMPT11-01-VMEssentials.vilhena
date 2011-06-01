using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Contracts.View;

namespace HttpReflector.Views
{
    public class ExceptionView : IView
    {
        public Exception Exception { get; set; }
    }
}
