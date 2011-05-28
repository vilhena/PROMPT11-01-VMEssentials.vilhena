using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Controllers.Exception
{
    public class InvalidContextModelException : ModelException
    {
        public string Context { get; set; }

        public InvalidContextModelException(string context)
            : base(string.Format("Context {0} does not exists",
            context))
        {
            this.Context = context;
        }

    }
}
