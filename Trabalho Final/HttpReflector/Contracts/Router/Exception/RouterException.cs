using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Contracts.Router.Exception
{
    public abstract class RouterException : System.Exception
    {
        protected RouterException(string msg) : base(msg) { }
    }
}
