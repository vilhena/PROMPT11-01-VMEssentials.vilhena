using HttpReflector.Contracts.Controller;
using HttpReflector.Contracts.Handler;
using HttpReflector.Contracts.UIBinder;

namespace HttpReflector.Controllers
{
    public class ReflectorController: IController
    {

        public void RegisterUI(IUIBinder binder)
        {
            throw new System.NotImplementedException();
        }

        public void RegisterHandlers(IHandlerLoader loader)
        {
            throw new System.NotImplementedException();
        }

        public void Start()
        {
            throw new System.NotImplementedException();
        }
    }
}
