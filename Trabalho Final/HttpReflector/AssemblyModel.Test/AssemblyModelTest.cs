using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using HttpReflector.Controllers;
using HttpReflector.Controllers.Exception;
using HttpReflector.Controllers.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssemblyModelTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class AssemblyModelTest
    {
        public AssemblyModelTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

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

        //Use TestInitialize to run code before running each test 
        public AssemblyModel Model { get; set; }
        [TestInitialize()]
        public void MyTestInitialize()
        {
            this.Model = new AssemblyModel();
        }



        [TestMethod]
        public void AddContextToAssemblyModel()
        {
            Model.AddContext("ContextTest1", @"..\..\..\Test\ContextTest1");
            var ctx = Model.ListContexts().Find(s => s.Name == "ContextTest1");
            Assert.AreEqual("ContextTest1", ctx.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPathAssemblyModelException))]
        public void AddContextWithInvalidPathThrowsInvalidPathException()
        {
            try
            {
                Model.AddContext("Invalid", @"X:\");
            }
            catch (InvalidPathAssemblyModelException ex)
            {
                Assert.AreEqual(@"X:\", ex.Path);
                throw;
            }
        }

        [TestMethod]
        public void ListAllContextReturnsAllContextModels()
        {
            Model.AddContext("ContextTest1", @"..\..\..\Test\ContextTest1");
            Model.AddContext("ContextTest2", @"..\..\..\Test\ContextTest2");
            var ctxs = Model.ListContexts();

            Assert.AreEqual(2,ctxs.Count);
            Assert.IsNotNull(ctxs.Find(s => s.Name == "ContextTest1"));
            Assert.IsNotNull(ctxs.Find(s => s.Name == "ContextTest2"));
        }

        [TestMethod]
        public void ListAllContextAssembliesContextModel()
        {
            Model.AddContext("ContextTest1", @"..\..\..\Test\ContextTest1");
            var asms = Model.ListContextAssemblies("ContextTest1");
            Assert.AreEqual(3,asms.Count);
            var vb = asms.Find(s => s.Name == "Microsoft.VisualBasic");
            var addinContract = asms.Find(s => s.Name == "System.AddIn.Contract");
            var addin = asms.Find(s => s.Name == "System.AddIn");
            Assert.IsNotNull(vb);
            Assert.IsNotNull(addinContract);
            Assert.IsNotNull(addin);
        }

        [TestMethod]
        public void GetContextContextModel()
        {
            Model.AddContext("ContextTest1", @"..\..\..\Test\ContextTest1");
            var asms = Model.GetContext("ContextTest1");
            Assert.IsNotNull(asms);
            Assert.AreEqual("ContextTest1",asms.Name);
            Assert.AreEqual(@"..\..\..\Test\ContextTest1",asms.Folder);
            Assert.AreEqual(3,asms.Assemblies.Count);
        }



        [TestMethod]
        [ExpectedException(typeof(InvalidContextModelException))]
        public void GetContextUsingInvalidContextThrowsInvalidContextExceptionContextModel()
        {
            Model.AddContext("ContextTest1", @"..\..\..\Test\ContextTest1");
            try
            {
                var asms = Model.GetContext("ContextTestInvalid");
            }
            catch (InvalidContextModelException ex)
            {
                Assert.AreEqual(@"ContextTestInvalid", ex.Context);
                throw;
            }
        }

        [TestMethod]
        public void GetContextAssemblyContextModel()
        {
            Model.AddContext("ContextTest1", @"..\..\..\Test\ContextTest1");
            var asms = Model.GetAssembly("ContextTest1", "Microsoft.VisualBasic");
            Assert.IsNotNull(asms);
            Assert.AreEqual("Microsoft.VisualBasic", asms.Name);
            Assert.AreEqual("Microsoft.VisualBasic, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",asms.FullName);
            Assert.IsNull(asms.PublicKey);
            Assert.AreEqual("v4.0.30319",asms.Version);
            Assert.AreEqual(8, asms.Namespaces.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidContextModelException))]
        public void GetAssemblyInvalidContextThrowsInvalidContextException()
        {
            Model.AddContext("ContextTest1", @"..\..\..\Test\ContextTest1");
            try
            {
                var asms = Model.GetAssembly("ContextTestInvalid", "Microsoft.VisualBasic");
            }
            catch (InvalidContextModelException ex)
            {
                Assert.AreEqual(@"ContextTestInvalid", ex.Context);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAssemblyModelException))]
        public void GetAssemblyInvalidContextThrowsInvalidAssemblyException()
        {
            Model.AddContext("ContextTest1", @"..\..\..\Test\ContextTest1");
            try
            {
                var asms = Model.GetAssembly("ContextTest1", "Microsoft.VisualBasic.Invalid");
            }
            catch (InvalidAssemblyModelException ex)
            {
                Assert.AreEqual(@"Microsoft.VisualBasic.Invalid", ex.Assembly);
                throw;
            }
        }
        
    }
}
