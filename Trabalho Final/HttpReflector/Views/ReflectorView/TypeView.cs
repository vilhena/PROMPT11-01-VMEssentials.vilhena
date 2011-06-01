using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Contracts.View;
using HttpReflector.Controllers.Model;

namespace HttpReflector.Views
{
    public class TypeView: IView
    {
        public ReflectType Type { get; set; }
        public List<ReflectMethod> Methods { get; set; }
        public List<ReflectMethod> Constructors { get; set; }
        public List<ReflectField> Fields { get; set; }
        public List<ReflectProperty> Properties { get; set; }
        public List<ReflectEvent> Events { get; set; }
    }
}
