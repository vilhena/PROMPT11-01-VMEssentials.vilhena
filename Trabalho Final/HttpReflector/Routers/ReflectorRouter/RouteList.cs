using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Contracts.Handler;
using HttpReflector.Contracts.Router;
using System.Text.RegularExpressions;
using HttpReflector.Contracts.Router.Exception;

namespace HttpReflector.Routers
{
    //make this generic
    public class RouteList<T> : IRouteContainer<T>
    {
        private readonly IDictionary<string, T> _routeList = new Dictionary<string, T>();

        public void Map(string pattern, T handler)
        {
            if (!_routeList.ContainsKey(pattern))
                this._routeList.Add(pattern, handler);
        }

        public RouteResult<T> Seek(string path)
        {
            foreach (var routePatterns in this._routeList.Keys)
            {
                var map = IsMatch(path, routePatterns);
                if (map != null)
                {
                    var result = new RouteResult<T>
                                     {
                                         Map = map,
                                         Handler = this._routeList[routePatterns]
                                     };
                    return result;
                }
                    
            }
            throw new CannotFindRouterException(path);
        }

        private static Dictionary<string, string> IsMatch(string path, string routePatterns)
        {
            var map = new Dictionary<string, string>();
            var listTokens = new Stack<string>(path.Split('/'));
            var routeTokens = new Stack<string>(routePatterns.Split('/'));
            
            if(listTokens.Count != routeTokens.Count)
                return null;

            foreach (var token in routeTokens)
            {
                var item = listTokens.Pop();

                if (token.Contains("{") && token.Contains("}"))
                {
                    //must be a name
                    if (item.Length == 0)
                        return null;
                }
                else
                {
                    if (token != item)
                        return null;

                }

                if (!map.ContainsKey(token))
                    map.Add(token, item);
            }

            return map;
        }

        public void UnMap(string pattern)
        {
            if (_routeList.ContainsKey(pattern))
                this._routeList.Remove(pattern);
        }



        public T Find(string pattern)
        {
            return _routeList.ContainsKey(pattern) ? _routeList[pattern] : default(T);
        }
    }
}
