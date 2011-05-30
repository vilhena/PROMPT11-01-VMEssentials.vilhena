using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using HttpReflector.Contracts.Handler;
using HttpReflector.Contracts.Router;
using HttpReflector.Contracts.Router.Exception;
using HttpReflector.Contracts.View;
using HttpReflector.Routers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReflectorRouter.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class RouterCollection
    {
        public RouterCollection()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test

        private IHandler _dummyHandler;
        private IRouteContainer<IHandler> _container;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            _dummyHandler = new MyDummyHandler();

            _container = new RouteList<IHandler>();
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        private class MyDummyHandler:IHandler
        {
            public IView Run()
            {
                return null;
            }
        }

        [TestMethod]
        public void CanAddRouteToRouteList()
        {
            _container.Map("/{ctx}/as/{assemblyName}", _dummyHandler );
            Assert.AreEqual(_container.Find("/{ctx}/as/{assemblyName}"), _dummyHandler);
        }

        [TestMethod]
        public void CanRemoveRouteFromRouteList()
        {
            _container.Map("/{ctx}/as/{assemblyName}", _dummyHandler);
            _container.UnMap("/{ctx}/as/{assemblyName}");
            Assert.IsNull(_container.Find("/{ctx}/as/{assemblyName}"));
        }

        [TestMethod]
        public void RouteWorksWithRootOnlyRouteList()
        {
            _container.Map("/", _dummyHandler);
            Assert.AreEqual(_container.Seek("/").Handler,_dummyHandler);
        }

        [TestMethod]
        public void RouteWorksWithRootCtxRouteList()
        {
            _container.Map("/{ctx}", _dummyHandler);
            Assert.IsNotNull(_container.Seek("/teste"));
        }

        [TestMethod]
        public void RouteWorksWithRootCtxAsRouteList()
        {
            _container.Map("/{ctx}/as", new MyDummyHandler());
            Assert.IsNotNull(_container.Seek("/teste/as"));
        }

        [TestMethod]
        public void RouteWorksWithRootCtxNsRouteList()
        {
            _container.Map("/{ctx}/ns", _dummyHandler);
            Assert.IsNotNull(_container.Seek("/teste/ns"));
        }

        [TestMethod]
        public void RouteWorksWithRootCtxAsAssemblyNameRouteList()
        {
            
            _container.Map("/{ctx}/as/{assemblyName}", _dummyHandler);
            Assert.IsNotNull(_container.Seek("/teste/as/assembly"));
        }

        [TestMethod]
        public void RouteWorksWithRootCtxNsNamespacePrefixRouteList()
        {
            _container.Map("/{ctx}/ns/{namespacePrefix}", _dummyHandler);
            Assert.IsNotNull(_container.Seek("/teste/ns/namespace"));
        }

        [TestMethod]
        public void RouteWorksWithRootCtxNsNamespaceShortnameRouteList()
        {
            _container.Map("/{ctx}/ns/{namespace}/{shortName}", _dummyHandler);
            Assert.IsNotNull(_container.Seek("/teste/ns/namespace/short"));
        }

        [TestMethod]
        public void RouteWorksWithRootCtxNsNamespaceShortnameMMethodNameRouteList()
        {
            _container.Map("/{ctx}/ns/{namespace}/{shortName}/m/{methodName}", _dummyHandler);
            Assert.IsNotNull(_container.Seek("/teste/ns/namespace/short/m/method"));
        }

        [TestMethod]
        public void RouteWorksWithRootCtxNsNamespaceShortnameCRouteList()
        {
            _container.Map("/{ctx}/ns/{namespace}/{shortName}/c", _dummyHandler);
            Assert.IsNotNull(_container.Seek("/teste/ns/namespace/short/c"));
        }

        [TestMethod]
        public void RouteWorksWithRootCtxNsNamespaceShortnameFFieldNameRouteList()
        {
            _container.Map("/{ctx}/ns/{namespace}/{shortName}/f/{fieldName}", _dummyHandler);
            Assert.IsNotNull(_container.Seek("/teste/ns/namespace/short/f/file"));
        }

        [TestMethod]
        public void RouteWorksWithRootCtxNsNamespaceShortnamePPropNameRouteList()
        {
            _container.Map("/{ctx}/ns/{namespace}/{shortName}/p/{propName}", _dummyHandler);
            Assert.IsNotNull(_container.Seek("/teste/ns/namespace/short/p/prop"));
        }

        [TestMethod]
        public void RouteWorksWithRootCtxNsNamespaceShortnameREventNameRouteList()
        {
            _container.Map("/{ctx}/ns/{namespace}/{shortName}/e/{eventName}", _dummyHandler);
            Assert.IsNotNull(_container.Seek("/teste/ns/namespace/short/e/evento"));
        }

        [TestMethod]
        public void RouteSeekReturnsValidHandlerNoRouteResult()
        {
            _container.Map("/{ctx}/ns/{namespace}/{shortName}/e/{eventName}", _dummyHandler);
            var result = _container.Seek("/Context1/ns/Visual.Basic/Form/e/OnLoad");
            Assert.AreEqual(_dummyHandler,result.Handler);
        }

        [TestMethod]
        [ExpectedException(typeof(CannotFindRouterException))]
        public void RouteSeekThrowsCannotFindRouterExceptionIfCannotFindPattern()
        {
            _container.Map("/{ctx}/ns/{namespace}/{shortName}/e/{eventName}", _dummyHandler);

            try
            {
                var result = _container.Seek("/Context1/ns/Visual.Basic/Form/e/OnLoad/Invalid");
            }
            catch (CannotFindRouterException ex)
            {
                Assert.AreEqual("/Context1/ns/Visual.Basic/Form/e/OnLoad/Invalid", ex.Path);
                throw;
            }
        }

        [TestMethod]
        public void RouteSeekReturnsAllKeysNoRouteResult()
        {
            _container.Map("/{ctx}/ns/{namespace}/{shortName}/e/{eventName}", _dummyHandler);
            var result = _container.Seek("/Context1/ns/Visual.Basic/Form/e/OnLoad");
            Assert.AreEqual("Context1", result.Map["{ctx}"]);
            Assert.AreEqual("ns", result.Map["ns"]);
            Assert.AreEqual("Visual.Basic", result.Map["{namespace}"]);
            Assert.AreEqual("Form", result.Map["{shortName}"]);
            Assert.AreEqual("e", result.Map["e"]);
            Assert.AreEqual("OnLoad", result.Map["{eventName}"]);
        }

    }
}
