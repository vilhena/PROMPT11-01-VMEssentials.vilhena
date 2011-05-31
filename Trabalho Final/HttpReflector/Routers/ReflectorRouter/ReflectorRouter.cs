using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Contracts.Handler;
using HttpReflector.Contracts.Router;
using HttpReflector.Handlers.MapBinders;

namespace HttpReflector.Routers
{
    public class ReflectorRouter: IRouter<IHandler>
    {
        private readonly IRouteContainer<IHandler> _route;
        private readonly IHandlerMapBinder _binder; 

        public ReflectorRouter()
        {
            //TODO: change this to RouteTree
            _route = new RouteList<IHandler>();
            _binder = new AttributeHandlerMapBinder();
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
            _binder.Bind(result.Map, result.Handler);
            return result.Handler;
        }
    }
}
