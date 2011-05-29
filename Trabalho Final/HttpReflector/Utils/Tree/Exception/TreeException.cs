using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Utils.Exception
{
    public abstract class TreeException: System.Exception
    {
        protected TreeException(string msg) : base(msg) { }
    }
}
