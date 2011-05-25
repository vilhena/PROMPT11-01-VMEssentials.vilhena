using HttpReflector.Controllers;
using HttpReflector.Contracts.Controller;

namespace HttpReflector.HttpReflectorSrv
{
    class Program
    {
        static void Main(string[] args)
        {
            IController controller = new ReflectorController();

            //TODO: RegisterHandlers using a folder
            //controller.LoadHandlers();
            //controller.LoadUI();
            //todo: start Assync with register callbacks
            controller.Start();

        }
    }
}
