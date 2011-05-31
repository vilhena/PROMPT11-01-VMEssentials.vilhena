using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Controllers.Model
{
    public class ReflectType : ReflectModel
    {
        public ReflectNamespace Namespace { get; set; }

        public ReflectAssembly Assembly { get; set; }

        public string FullName { get; set; }

        public List<ReflectMethod> Methods { get; set; }

        public List<ReflectField> Fields { get; set; }

        public List<ReflectProperty> Properties { get; set; }

        public List<ReflectMethod> Contructors { get; set; }

        public List<ReflectEvent> Events { get; set; }
    }
}
