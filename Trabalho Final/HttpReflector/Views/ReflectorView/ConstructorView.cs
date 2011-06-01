using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Contracts.View;
using HttpReflector.Controllers.Model;

namespace HttpReflector.Views
{
    public class ConstructorView : IView
    {
        public ReflectType Type { get; set; }
        public List<ReflectMethod> Constructors { get; set; }
    }
}
