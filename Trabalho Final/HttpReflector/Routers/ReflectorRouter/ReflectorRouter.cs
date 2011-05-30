using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Contracts.Handler;
using HttpReflector.Contracts.Router;

namespace HttpReflector.Routers
{
    public class ReflectorRouter: IRouter<IHandler>
    {
        private readonly IRouteContainer<IHandler> _route;

        public ReflectorRouter()
        {
            //TODO: change this to RouteMap
            _route = new RouteList<IHandler>();
        }

        public void RegisterRoute(string route, IHandler routeHandler)
        {
            this._route.Map(route, routeHandler);
        }

        public void UnregisterRoute(string route)
        {
            this._route.UnMap(route);
        }

        public IHandler Route(string path)
        {
            var result = this._route.Seek(path);
            // TODO Fill Handler with Map

            return result.Handler;
        }
    }
}
