using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Controllers.Exception
{
    public abstract class ModelException : System.Exception
    {
        protected ModelException(string msg) : base(msg) { }
    }
}
