using System.Collections.Generic;
using HttpReflector.Utils.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HttpReflector.Utils.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class TreeTest
    {
        public TreeTest()
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
        public void InitNewTreeWithRoot()
        {
            var tree = new Tree<string,int>();
            tree.Add(new List<string>() {@"\"}, 1);
            var value = tree.GetValue(new List<string>()
                                          {
                                              @"\"
                                          });
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void ContainsResultsTrueRootValueOnNewTree()
        {
            var tree = new Tree<string, int>();
            tree.Add(new List<string>() { @"\" }, 1);

            var value = tree.ContainsKey(new List<string>()
                                          {
                                              @"\"
                                          });
            Assert.IsTrue(value);
        }




        [TestMethod]
        public void CanFindRootValueOnNewTree()
        {
            var tree = new Tree<string, int>();
            tree.Add(new List<string>() { @"\" }, 1);

            var value = tree.GetValue(new List<string>()
                                          {
                                              @"\"
                                          });
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundTreeException<string>))]
        public void CanFindRootValueThrowsKeyNotFoundTreeExceptionOnNewTree()
        {
            var tree = new Tree<string, int>();
            tree.Add(new List<string>() { @"\" }, 1);

            try
            {
                var value = tree.GetValue(new List<string>()
                                          {
                                              @"X"
                                          });
            }
            catch (KeyNotFoundTreeException<string> ex)
            {
                Assert.AreEqual("X", ex.Key);
                throw;
            }
        }

        [TestMethod]
        public void AddValueToTree()
        {
            var tree = new Tree<string, int>();
            tree.Add(new List<string>() { @"\" }, 1);

            var path = new List<string>()
                           {
                               @"\",
                               @"{ctx}",
                               @"{ns}",
                               @"{namespace}",
                               @"{shortname}",
                           };
            var path2 = new List<string>()
                           {
                               @"\",
                               @"{ctx}",
                               @"{as}",
                               @"{assemblyName}"
                           };

            tree.Add(path,10);
            tree.Add(path2,50);

            var value = tree.GetValue(path);

            var value2 = tree.GetValue(path2);

            Assert.AreEqual(10,value);
            Assert.AreEqual(50, value2);

        }

        [TestMethod]
        public void CanAddValueToEmpyTree()
        {
            var tree = new Tree<string, int>();

            var path = new List<string>()
                           {
                               @"\",
                               @"{ctx}",
                               @"{ns}",
                               @"{namespace}",
                               @"{shortname}",
                           };
            var path2 = new List<string>()
                           {
                               @"\",
                               @"{ctx}",
                               @"{as}",
                               @"{assemblyName}"
                           };

            tree.Add(path, 10);
            tree.Add(path2, 50);

            var value = tree.GetValue(path);

            var value2 = tree.GetValue(path2);

            Assert.AreEqual(10, value);
            Assert.AreEqual(50, value2);
        }

    }
}
