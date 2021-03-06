﻿using HttpReflector.Controllers;
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
            router.RegisterRoute("/{ctx}/ns/{namespace}/{shortName}", new TypeHandler());
            router.RegisterRoute("/{ctx}/ns/{namespace}/{shortName}/m/{methodName}", new MethodHandler());
            router.RegisterRoute("/{ctx}/ns/{namespace}/{shortName}/c", new ConstructorHandler());
            router.RegisterRoute("/{ctx}/ns/{namespace}/{shortName}/f/{fieldName}", new FieldHandler());
            router.RegisterRoute("/{ctx}/ns/{namespace}/{shortName}/p/{propName}", new PropertyHandler());
            router.RegisterRoute("/{ctx}/ns/{namespace}/{shortName}/e/{eventName}", new EventHandler());
            
            //TODO:deixar de ser estatico
            AssemblyModel.AddContext("ContextTest1", @"..\..\..\..\Test\ContextTest1");
            AssemblyModel.AddContext("ContextTest2", @"..\..\..\..\Test\ContextTest2");

            AssemblyModel.AddContext("System32",
                                     @"C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0");

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
