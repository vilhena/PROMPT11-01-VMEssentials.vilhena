using HttpReflector.Contracts.Handler;
using HttpReflector.Contracts.Router;
using HttpReflector.Contracts.UIBinder;

namespace HttpReflector.Contracts.Controller
{
    public interface IController
    {
        void RegisterUI(IUIBinder binder);
        void RegisterHandlers(IHandlerLoader loader);
        void RegisterRouter(IRouter<IHandler> router);
        void Start();
        void Stop();
    }
}
