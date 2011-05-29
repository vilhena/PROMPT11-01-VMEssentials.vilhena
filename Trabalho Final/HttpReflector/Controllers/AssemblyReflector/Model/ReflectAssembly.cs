using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Utils;

namespace HttpReflector.Controllers.Model
{
    public class ReflectAssembly: ReflectModel
    {
        public ReflectContext Context { get; set; }

        public string FullName { get; set; }

        public string Version { get; set; }

        public object PublicKey { get; set; }

        public Dictionary<string, ReflectNamespace> Namespaces { get; set; }
    }
}
