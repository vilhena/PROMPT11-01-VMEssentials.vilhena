using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Contracts.Handler
{
    public interface IHandlerMapBinder
    {
        void Bind(Dictionary<string,string> map, IHandler handler);
    }
}
