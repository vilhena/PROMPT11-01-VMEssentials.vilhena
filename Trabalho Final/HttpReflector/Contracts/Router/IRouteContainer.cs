using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Contracts.Handler;

namespace HttpReflector.Contracts.Router
{
    public interface IRouteContainer<T>
    {
        void Map(string pattern, T handler);
        T Find(string pattern);
        RouteResult<T> Seek(string route);
        void UnMap(string pattern);
    }
}
