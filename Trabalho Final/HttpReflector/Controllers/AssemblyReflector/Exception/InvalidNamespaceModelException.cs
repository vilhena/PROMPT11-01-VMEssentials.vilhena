using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Controllers.Exception
{
    public class InvalidNamespaceModelException:ModelException
    {
        public string Context { get; set; }
        public string Namespace { get; set; }

        public InvalidNamespaceModelException(string context, string shortName)
            : base(string.Format("Namespace {0} does not exists on {1} context",
            shortName, context))
        {
            this.Context = context;
            this.Namespace = shortName;
        }
    }
}
