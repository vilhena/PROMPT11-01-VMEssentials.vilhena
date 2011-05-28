using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Controllers.Model
{
    public class ReflectType : ReflectModel
    {

        public ReflectNamespace NameSpace { get; set; }

        public ReflectAssembly Assembly { get; set; }

        public Dictionary<string, ReflectMethod> Methods { get; set; }

        public Dictionary<string, ReflectField> Fields { get; set; }

        public Dictionary<string, ReflectProperty> Properties { get; set; }
    }
}
