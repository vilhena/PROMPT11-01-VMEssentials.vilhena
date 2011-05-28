using System.Collections.Generic;

namespace HttpReflector.Controllers.Model
{
    public class ReflectContext : ReflectModel
    {
        public string Folder { get; set; }

        public Dictionary<string,ReflectAssembly> Assemblies { get; set; }
    }
}
