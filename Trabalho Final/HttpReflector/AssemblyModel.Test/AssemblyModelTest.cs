using System;
using System.Reflection;
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
        
        [TestInitialize()]
        public void MyTestInitialize()
        {
        }



        [TestMethod]
        public void AddContextToAssemblyModel()
        {
            AssemblyModel.AddContext("ContextTest1", @"..\..\..\Test\ContextTest1");
            var ctx = AssemblyModel.ListContexts().Find(s => s.Name == "ContextTest1");
            Assert.AreEqual("ContextTest1", ctx.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPathAssemblyModelException))]
        public void AddContextWithInvalidPathThrowsInvalidPathException()
        {
            try
            {
                AssemblyModel.AddContext("Invalid", @"X:\");
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
            AssemblyModel.AddContext("ContextTest1", @"..\..\..\Test\ContextTest1");
            AssemblyModel.AddContext("ContextTest2", @"..\..\..\Test\ContextTest2");
            var ctxs = AssemblyModel.ListContexts();

            Assert.AreEqual(2,ctxs.Count);
            Assert.IsNotNull(ctxs.Find(s => s.Name == "ContextTest1"));
            Assert.IsNotNull(ctxs.Find(s => s.Name == "ContextTest2"));
        }

        

        [TestMethod]
        public void GetContextContextModel()
        {
            AssemblyModel.AddContext("ContextTest1", @"..\..\..\Test\ContextTest1");
            var asms = AssemblyModel.GetContext("ContextTest1");
            Assert.IsNotNull(asms);
            Assert.AreEqual("ContextTest1",asms.Name);
            Assert.AreEqual(@"..\..\..\Test\ContextTest1",asms.Folder);
            Assert.AreEqual(3,asms.Assemblies.Count);
        }



        [TestMethod]
        [ExpectedException(typeof(InvalidContextModelException))]
        public void GetContextUsingInvalidContextThrowsInvalidContextExceptionContextModel()
        {
            AssemblyModel.AddContext("ContextTest1", @"..\..\..\Test\ContextTest1");
            try
            {
                var asms = AssemblyModel.GetContext("ContextTestInvalid");
            }
            catch (InvalidContextModelException ex)
            {
                Assert.AreEqual(@"ContextTestInvalid", ex.Context);
                throw;
            }
        }

        [TestMethod]
        public void ListAllContextAssembliesContextModel()
        {
            AssemblyModel.AddContext("ContextTest1", @"..\..\..\Test\ContextTest1");
            var asms = AssemblyModel.ListContextAssemblies("ContextTest1");
            Assert.AreEqual(3, asms.Count);
            var vb = asms.Find(s => s.Name == "Microsoft.VisualBasic");
            var addinContract = asms.Find(s => s.Name == "System.AddIn.Contract");
            var addin = asms.Find(s => s.Name == "System.AddIn");
            Assert.IsNotNull(vb);
            Assert.IsNotNull(addinContract);
            Assert.IsNotNull(addin);
        }

        [TestMethod]
        public void GetContextAssemblyContextModel()
        {
            AssemblyModel.AddContext("ContextTest1", @"..\..\..\Test\ContextTest1");
            var asms = AssemblyModel.GetAssembly("ContextTest1", "Microsoft.VisualBasic");
            Assert.IsNotNull(asms);
            Assert.AreEqual("Microsoft.VisualBasic", asms.Name);
            Assert.AreEqual("Microsoft.VisualBasic, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",asms.FullName);
            Assert.IsNotNull(asms.PublicKey);
            Assert.AreEqual("v4.0.30319",asms.Version);
            
            //Assert.AreEqual(8, asms.Namespaces.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidContextModelException))]
        public void GetAssemblyInvalidContextThrowsInvalidContextException()
        {
            AssemblyModel.AddContext("ContextTest1", @"..\..\..\Test\ContextTest1");
            try
            {
                var asms = AssemblyModel.GetAssembly("ContextTestInvalid", "Microsoft.VisualBasic");
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
            AssemblyModel.AddContext("ContextTest1", @"..\..\..\Test\ContextTest1");
            try
            {
                var asms = AssemblyModel.GetAssembly("ContextTest1", "Microsoft.VisualBasic.Invalid");
            }
            catch (InvalidAssemblyModelException ex)
            {
                Assert.AreEqual(@"Microsoft.VisualBasic.Invalid", ex.Assembly);
                Assert.AreEqual(@"ContextTest1", ex.Context);
                throw;
            }
        }

        [TestMethod]
        public void ListAllContextNamespacesContextModel()
        {
            AssemblyModel.AddContext("ContextTest1", @"..\..\..\Test\ContextTest1");
            var ns = AssemblyModel.ListNamespaces("ContextTest1");

            Assert.AreEqual(20, ns.Count);
        }

        [TestMethod]
        public void GetContextNamespaceContextModel()
        {
            AssemblyModel.AddContext("ContextTest1", @"..\..\..\Test\ContextTest1");
            var ns = AssemblyModel.GetNamespace("ContextTest1", "Microsoft.VisualBasic.FileIO");
            var assemebly = AssemblyModel.GetAssembly("ContextTest1", "Microsoft.VisualBasic");

            Assert.IsNotNull(ns);
            Assert.AreEqual("Microsoft.VisualBasic.FileIO",ns.Name);
            Assert.AreEqual(assemebly,ns.Assembly);
            Assert.AreEqual(17,ns.Types.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNamespaceModelException))]
        public void GetContextInvalidNamespaceThrowsInvalidNamespaceModelExceptio()
        {
            AssemblyModel.AddContext("ContextTest1", @"..\..\..\Test\ContextTest1");
            try
            {
                var asms = AssemblyModel.GetNamespace("ContextTest1", "Microsoft.VisualBasic.Invalid");
            }
            catch (InvalidNamespaceModelException ex)
            {
                Assert.AreEqual(@"Microsoft.VisualBasic.Invalid", ex.Namespace);
                Assert.AreEqual(@"ContextTest1", ex.Context);
                throw;
            }
        }

        [TestMethod]
        public void GetTypeContextModel()
        {
            var asm = Assembly.LoadFrom(@"..\..\..\Test\ContextTest1\Microsoft.VisualBasic.dll");
            Type asmType = asm.GetType("Microsoft.VisualBasic.CompilerServices.VBInputBox");

            AssemblyModel.AddContext("ContextTest1", @"..\..\..\Test\ContextTest1");

            var type = AssemblyModel.GetCtsType("ContextTest1", "Microsoft.VisualBasic.CompilerServices", "VBInputBox");

            Assert.IsNotNull(type);
            Assert.AreEqual("Microsoft.VisualBasic.CompilerServices", type.Namespace.Name);
            Assert.AreEqual("ContextTest1", type.Assembly.Context.Name);
            Assert.AreEqual(0,type.Contructors.Count);
            Assert.AreEqual(91, type.Events.Count);
            Assert.AreEqual(1, type.Fields.Count);
            Assert.AreEqual("Microsoft.VisualBasic.CompilerServices.VBInputBox", type.FullName);
            Assert.AreEqual(496, type.Methods.Count);
            Assert.AreEqual(126,type.Properties.Count);

            foreach (var fieldInfo in asmType.GetFields())
            {
                var info = fieldInfo;
                var ctor = type.Fields.Find(s => s.Name == info.Name);

                Assert.IsNotNull(ctor);
                Assert.AreEqual(info.Name, ctor.Name);
                Assert.AreEqual(info.FieldType.Name, ctor.Type.Name);

            }
            foreach (var propertyInfo in asmType.GetProperties())
            {
                var info = propertyInfo;
                var ctor = type.Properties.Find(s => s.Name == info.Name);

                Assert.IsNotNull(ctor);
                Assert.AreEqual(info.Name,ctor.Name);
                Assert.AreEqual(info.PropertyType.Name, ctor.Type.Name);
            }

            foreach (var constructorInfo in asmType.GetConstructors())
            {
                var info = constructorInfo;
                var ctor =
                    type.Contructors.Find(s => s.Name == info.Name);

                Assert.IsNotNull(ctor);
                Assert.AreEqual(constructorInfo.Name, ctor.Name);
                Assert.AreEqual(constructorInfo.GetParameters().Count(), ctor.Parameters.Count);
            }
            foreach (var methodInfo in asmType.GetMethods())
            {
                var info = methodInfo;
                var lmtor =
                    type.Methods.FindAll(s => (s.Name == info.Name)
                                           && (s.Parameters.Count == info.GetParameters().Count()));

                var info2 = methodInfo;
                var mtor = lmtor.Find(s => s.Parameters.TrueForAll(
                    t => info2.GetParameters().ToList().Find(p => p.ParameterType.Name == t.Type.Name) != null));


                Assert.IsNotNull(mtor);
                Assert.AreEqual(methodInfo.Name, mtor.Name);
                Assert.AreEqual(methodInfo.GetParameters().Count(), mtor.Parameters.Count);
                Assert.AreEqual(methodInfo.ReturnType.Name, mtor.Return.Name);

                foreach (var parameterInfo in methodInfo.GetParameters())
                {
                    var info1 = parameterInfo;
                    var ptor =
                        mtor.Parameters.Find(s => s.Name == info1.Name);

                    Assert.IsNotNull(ptor);
                    Assert.AreEqual(parameterInfo.Name, ptor.Name);
                    Assert.AreEqual(parameterInfo.ParameterType.Name, ptor.Type.Name);
                }

            }



        }

    }
}
