using System.Collections.Generic;
using HttpReflector.Utils;

namespace HttpReflector.Controllers.Model
{
    public class ReflectContext : ReflectModel
    {
        public string Folder { get; set; }

        public Dictionary<string,ReflectAssembly> Assemblies { get; set; }
        public Tree<string,ReflectNamespace> Namespaces { get; set; }
    }
}
