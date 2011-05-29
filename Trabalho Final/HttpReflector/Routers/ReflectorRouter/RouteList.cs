using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Contracts.Handler;
using HttpReflector.Contracts.Router;
using System.Text.RegularExpressions;

namespace HttpReflector.Routers
{
    //make this generic
    public class RouteList<T> : IRouteContainer<T>
    {
        //TODO:lista de componentes de pattern para a comparacao ser mais facil
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
                if (IsMatch(path, routePatterns))
                {
                    var result = new RouteResult<T>
                                     {
                                         Handler = this._routeList[routePatterns]
                                     };
                    return result;
                }
                    
            }
            return null;

        }

        private static bool IsMatch(string path, string routePatterns)
        {
            var listTokens = new Stack<string>(path.Split('/'));
            var routeTokens = new Stack<string>(routePatterns.Split('/'));
            
            if(listTokens.Count != routeTokens.Count)
                return false;

            for(int i = 0; i < routeTokens.Count ; ++i )
            {
                var token = routeTokens.Pop();
                var item = listTokens.Pop();

                if (token.Contains("{") && token.Contains("}"))
                {
                    //must be a name
                    if (item.Length == 0)
                        return false;
                }
                else
                {
                    if (token != item)
                        return false;
                }
            }

            return true;
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
