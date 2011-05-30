using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using HttpReflector.Controllers.Model;
using HttpReflector.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReflectorView.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ReflectorViewTest
    {
        public ReflectorViewTest()
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
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void BindRootView()
        {
            var rootView = new RootView()
                               {
                                   Contexts = new List<ReflectContext>()
                               };
            rootView.Contexts.Add(new ReflectContext()
                                      {
                                          Folder = "C:\teste",
                                          Name = "Context1"
                                      }
                );
            rootView.Contexts.Add(new ReflectContext()
            {
                Folder = "C:\teste2",
                Name = "Context2"
            }
                );

            ViewBinder.RootFolder = @"..\..\..\Views\ReflectorView.Test\";
            var output = ViewBinder.BindView(rootView);
            
            var sb = new StringBuilder();

            using (TextReader reader = File.OpenText(ViewBinder.RootFolder + "RootViewOut.txt"))
            {
                sb.Append(reader.ReadToEnd());
            }

            Assert.AreEqual(sb.ToString(),output);
        }
    }
}
