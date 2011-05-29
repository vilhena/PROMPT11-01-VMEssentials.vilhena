using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Controllers.Exception
{
    public class InvalidAssemblyModelException : ModelException
    {
        public string Context { get; set; }
        public string Assembly { get; set; }

        public InvalidAssemblyModelException(string context, string assembly)
            : base(string.Format("Assembly {0} does not exists on {1} context",
            assembly, context))
        {
            this.Context = context;
            this.Assembly = assembly;
        }

    }
}
