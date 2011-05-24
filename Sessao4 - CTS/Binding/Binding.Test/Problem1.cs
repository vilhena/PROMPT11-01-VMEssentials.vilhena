using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace Binding.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class Problem1
    {
        public Problem1()
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

        class A
        {
            public int AnInteger { get; set; }
            public string AString { get; set; }
            public int AnFieldInteger = -1;
            public List<string> AnList;
        }


        [TestMethod]
        public void cann_bind_properties_to_A()
        {
            var binder = new Binder<A>();
            var pairs = new KeyValuePair<string, string>[]{
                new KeyValuePair<string,string>("AnInteger","2"),
            new KeyValuePair<string,string>("AString","abc")
            };

            var a = binder.Bind(pairs);

            Assert.AreEqual(2, a.AnInteger);
            Assert.AreEqual("abc", a.AString);
        }

        [TestMethod]
        public void cann_bind_fields_to_A()
        {
            var binder = new Binder<A>();
            var pairs = new KeyValuePair<string, string>[]{
                new KeyValuePair<string,string>("AnFieldInteger","10")
            };

            var a = binder.Bind(pairs);

            Assert.AreEqual(10, a.AnFieldInteger);
        }

        [TestMethod]
        [ExpectedException(typeof(NotPrimitiveMemberBinderException))]
        public void cannot_bind_complex_data_to_A()
        {
            var binder = new Binder<A>();
            var pairs = new KeyValuePair<string, string>[]{
                new KeyValuePair<string,string>("AnList",null)
            };
            try
            {
                var a = binder.Bind(pairs);
            }
            catch (NotPrimitiveMemberBinderException ex)
            {
                Assert.AreEqual(typeof(A).GetField("AnList"), ex.MemberInfo);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InexistentMemberBinderException))]
        public void cannot_bind_inexistent_property_to_A()
        {
            var binder = new Binder<A>();
            var pairs = new KeyValuePair<string, string>[]{
                new KeyValuePair<string,string>("AnInteger","10")
            };
            try
            {
                var a = binder.Bind(pairs);
            }
            catch (InexistentMemberBinderException ex)
            {
                Assert.AreEqual("AnInteger", ex.MemberName);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InexistentMemberBinderException))]
        public void cannot_bind_inexistent_field_to_A()
        {
            var binder = new Binder<A>();
            var pairs = new KeyValuePair<string, string>[]{
                new KeyValuePair<string,string>("AnFieldInteger","10")
            };
            try
            {
                var a = binder.Bind(pairs);
            }
            catch (InexistentMemberBinderException ex)
            {
                Assert.AreEqual("AnFieldInteger", ex.MemberName);
                throw;
            }
        }


    }
}
