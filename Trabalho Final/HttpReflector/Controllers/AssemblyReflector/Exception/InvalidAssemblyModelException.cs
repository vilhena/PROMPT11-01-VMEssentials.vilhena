using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Controllers.Exception
{
    public class InvalidAssemblyModelException : ModelException
    {
        public string Assembly { get; set; }

        public InvalidAssemblyModelException(string assembly)
            : base(string.Format("Assembly {0} does not exists",
            assembly))
        {
            this.Assembly = assembly;
        }

    }
}
