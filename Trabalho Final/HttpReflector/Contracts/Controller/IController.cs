using HttpReflector.Contracts.Handler;
using HttpReflector.Contracts.UIBinder;

namespace HttpReflector.Contracts.Controller
{
    public interface IController
    {
        void RegisterUI(IUIBinder binder);
        void RegisterHandlers(IHandlerLoader loader);
        void Start();
    }
}
