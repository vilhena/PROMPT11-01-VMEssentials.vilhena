using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Controllers.Model
{
    public class ReflectNamespace:ReflectModel
    {
        public ReflectAssembly Assembly { get; set; }
        public Dictionary<string, Lazy<ReflectType>> Types { get; set; }
    }
}
