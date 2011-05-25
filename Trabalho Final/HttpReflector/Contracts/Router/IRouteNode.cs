using HttpReflector.Contracts.Handler;

namespace HttpReflector.Contracts.Router
{
    public interface IRouteNode
    {
        string Route { get; set; }
        IHandler Handler { get; set; }
    }
}
