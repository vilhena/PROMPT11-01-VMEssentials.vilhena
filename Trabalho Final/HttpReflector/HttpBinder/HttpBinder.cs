using System;
using System.Net;
using System.Text;
using System.Threading;
using HttpReflector.Contracts.UIBinder;

namespace HttpReflector.UIBinders
{
    public class HttpBinder:IUIBinder
    {
        private HttpListener _httpListener;
        public WaitCallback Callback { get; set; }

        public HttpBinder()
        {
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add("http://127.0.0.1:8080/");

            //TODO: REMOVE THIS ONLY FOR TEST
            Callback = new WaitCallback(ProcessRequest);
        }

        public void Start()
        {
            if (Callback == null)
                throw new NotImplementedException("InvalidCallbackException");

            _httpListener.Start();

            ThreadPool.QueueUserWorkItem(Run, null);
        }

        private void Run(object contextobj)
        {
            while (_httpListener.IsListening)
                try
                {
                    HttpListenerContext request = _httpListener.GetContext();
                    ThreadPool.QueueUserWorkItem(Callback, request);
                }
                catch (HttpListenerException)
                {
                    break;
                }
                catch (InvalidOperationException)
                {
                    break;
                }
        }

        public void Stop()
        {
            _httpListener.Stop();
        }

        public void ProcessRequest(object contextobj)
        {
            try
            {
                var context = (HttpListenerContext) contextobj;
                

                if (context.Request.HttpMethod.Equals("GET"))
                {
                    var msg = context.Request.HttpMethod + " " + context.Request.Url;
       
                    byte[] b = Encoding.UTF8.GetBytes(msg);
                    context.Response.ContentLength64 = b.Length;
                    context.Response.OutputStream.Write(b, 0, b.Length);
                    context.Response.StatusCode = 200;
                }
                else
                {
                    context.Response.StatusCode = 501;
                }
                context.Response.Close();
            }
            catch (Exception ex)
            {
                //TODO: LOG ERRORS
                Console.WriteLine("Request error: " + ex);
            }
        }


    }
}
