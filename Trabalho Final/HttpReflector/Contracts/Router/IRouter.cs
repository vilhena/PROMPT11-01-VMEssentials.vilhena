using HttpReflector.Contracts.Handler;
namespace HttpReflector.Contracts.Router
{
    public interface IRouter<T> 
    {
        void RegisterRoute(string route, T routeHandler);
        void UnregisterRoute(string route);
        T Route(string route);
    }
}
