using HttpReflector.Controllers;
using HttpReflector.Contracts.Controller;
using HttpReflector.Handlers;
using HttpReflector.Routers;
using HttpReflector.UIBinders;
using HttpReflector.Views;

namespace HttpReflector.HttpReflectorSrv
{
    class Program
    {
        static void Main(string[] args)
        {
            IController controller = new ReflectorController();

            var router = new ReflectorRouter();
            router.RegisterRoute("/",new RootHandler());
            router.RegisterRoute("/{ctx}", new ContextHandler());
            router.RegisterRoute("/{ctx}/as", new ContextAssemblyHandler());
            router.RegisterRoute("/{ctx}/ns", new ContextNamespaceHandler());
            router.RegisterRoute("/{ctx}/as/{assemblyName}", new AssemblyHandler());
            router.RegisterRoute("/{ctx}/ns/{namespacePrefix}", new NamespaceHandler());

            AssemblyModel.AddContext("ContextTest1", @"..\..\..\..\Test\ContextTest1");
            AssemblyModel.AddContext("ContextTest2", @"..\..\..\..\Test\ContextTest2");

            ViewBinder.RootFolder = @"..\..\..\..\Test\Views\";

            var ui = new HttpBinder();

            //TODO: RegisterHandlers using a folder
            controller.RegisterUI(ui);
            controller.RegisterRouter(router);
            //todo: start Assync with register callbacks
            controller.Start();
        }
    }
}
