using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Contracts.Router;
using HttpReflector.Utils;

namespace HttpReflector.Routers
{
    public class RouteTree<T>: IRouteContainer<T>
    {
        private readonly Tree<string, T> _routeList = new Tree<string, T>();


        public void Map(string pattern, T handler)
        {
            var split = pattern.Split('\\');
            if (!_routeList.ContainsKey(split))
                this._routeList.Add(split, handler);
        }

        public RouteResult<T> Seek(string path)
        {
            // TODO: convert string to patterns??
            return null;

        }

        private static bool IsMatch(string path, string routePatterns)
        {
            var listTokens = new Stack<string>(path.Split('/'));
            var routeTokens = new Stack<string>(routePatterns.Split('/'));

            if (listTokens.Count != routeTokens.Count)
                return false;

            for (int i = 0; i < routeTokens.Count; ++i)
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
            //Not Implemented
        }



        public T Find(string pattern)
        {
            //Not Implemented
            return default(T);
        }
    }
}
