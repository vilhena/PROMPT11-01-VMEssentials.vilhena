using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using HttpReflector.Contracts.UIBinder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HttpReflector.UIBinders.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class HttpBinderTest
    {
        public HttpBinderTest()
        {
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
        
        
        // Use TestInitialize to run code before running each test 

        static HttpBinder _server;
        [ClassInitialize]
        public static void ClassStart(TestContext context)
        {
            _server = new HttpBinder();
            _server.Callback = _server.ProcessRequest;
            _server.Start();
        }

        
        [ClassCleanup]
        public static void MyTestCleanup()
        {
            _server.Stop();
        }

        private static string ReadResponse(HttpWebResponse response)
        {
            var data = new StringBuilder();

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                while (reader.Peek() != -1)
                {
                    data.Append(reader.ReadLine());
                }
            }
            return data.ToString();
        }

        //
        #endregion

        [TestMethod]
        public void HttpRespondsToGet()
        {
            var url = new Uri("http://localhost:8080");
            var request = (HttpWebRequest)
                          WebRequest.Create(url);

            var response = (HttpWebResponse)
                           request.GetResponse();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("GET http://localhost:8080/", ReadResponse(response));
        }

        [TestMethod]
        public void HttpDoNotRespondsToPut()
        {
            var url = new Uri("http://localhost:8080");
            var request = (HttpWebRequest)
                          WebRequest.Create(url);
            request.Method = "PUT";
            request.ContentLength = 0;

            try
            {
                var response = (HttpWebResponse)
                           request.GetResponse();

            }
            catch (WebException ex)
            {
                Assert.AreEqual(typeof(HttpWebResponse),ex.Response.GetType());
                Assert.AreEqual(HttpStatusCode.NotImplemented, ((HttpWebResponse)ex.Response).StatusCode);
            }
            
            
        }


    }
}
