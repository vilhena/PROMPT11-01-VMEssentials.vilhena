using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using HttpReflector.Controllers;
using HttpReflector.Handlers;
using HttpReflector.Handlers.Exceptions;
using HttpReflector.Handlers.MapBinders;
using HttpReflector.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReflectorHandler.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ReflectorHandlerTest
    {
        public ReflectorHandlerTest()
        {
            AssemblyModel.AddContext("ContextTest1", @"..\..\..\Test\ContextTest1");
            AssemblyModel.AddContext("ContextTest2", @"..\..\..\Test\ContextTest2");
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
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void RootHandlerRunReturnsRootView()
        {
            var handler = new RootHandler();

            var result = handler.Run();
            Assert.AreEqual(typeof(RootView),result.GetType());

            var view = (RootView) result;
            Assert.AreEqual(2,view.Contexts.Count);
            Assert.AreEqual("ContextTest1",view.Contexts[0].Name);
            Assert.AreEqual("ContextTest2", view.Contexts[1].Name);
        }

        [TestMethod]
        public void ContextHandlerRunReturnsContextView()
        {
            var handler = new ContextHandler {Context = "ContextTest1"};

            var result = handler.Run();
            Assert.AreEqual(typeof (ContextView), result.GetType());

            var view = (ContextView) result;
            Assert.AreEqual("ContextTest1", view.Context.Name);
            Assert.AreEqual(3, view.Assemblies.Count);
            Assert.AreEqual(20, view.Namespaces.Count);
        }

        [TestMethod]
        public void AttributeHandlerMapBinderContextHandlerTest()
        {
            var binder = new AttributeHandlerMapBinder();
            var handler = new ContextHandler();
            var map = new Dictionary<string, string> {{"{ctx}", "ContextTest1"}};

            binder.Bind(map, handler);

            Assert.AreEqual("ContextTest1", handler.Context);
        }

        [TestMethod]
        [ExpectedException(typeof(MapKeyNotFoundMapBinderException))]
        public void AttributeHandlerMapBinderThrowsMapKeyNotFoundMapBinderException()
        {
            var binder = new AttributeHandlerMapBinder();
            var handler = new ContextHandler();
            //empty map
            var map = new Dictionary<string, string>();
            try
            {
                binder.Bind(map, handler);
            }
            catch (MapKeyNotFoundMapBinderException ex)
            {
                Assert.AreEqual("{ctx}",ex.ExpectedKey);
                throw;
            }
        }
    }
}
