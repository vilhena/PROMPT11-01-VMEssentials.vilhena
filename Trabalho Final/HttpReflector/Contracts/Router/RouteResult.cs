using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Contracts.Router
{
    public sealed class RouteResult<T>
    {
        public Dictionary<string,string> Map { get; set; }
        public T Handler { get; set; }
    }
}
