using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Controllers.Exception
{
    public class InvalidCtsTypeModelException : ModelException
    {
        public string Context { get; set; }
        public string Assembly { get; set; }
        public string CtsType { get; set; }

        public InvalidCtsTypeModelException(string context, string assembly, string ctsType)
            : base(string.Format("Type {0} does not exists on Assembly {1} on {2} context",
            ctsType,assembly, context))
        {
            this.Context = context;
            this.Assembly = assembly;
            this.CtsType = ctsType;
        }

    }
}
