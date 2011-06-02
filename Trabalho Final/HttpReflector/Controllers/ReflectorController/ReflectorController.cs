using System;
using System.Net;
using System.Text;
using System.Threading;
using HttpReflector.Contracts.Controller;
using HttpReflector.Contracts.Handler;
using HttpReflector.Contracts.Router;
using HttpReflector.Contracts.UIBinder;
using HttpReflector.UIBinders;
using HttpReflector.Views;

namespace HttpReflector.Controllers
{
    public class ReflectorController: IController
    {
        private IUIBinder Binder { get; set; }
        private IRouter<IHandler> Router { get; set; }
        private bool _started = false;

        public void RegisterUI(IUIBinder binder)
        {
            Binder = binder;
            binder.Callback = new WaitCallback(ProcessRequest);
        }

        
        public void RegisterRouter(IRouter<IHandler> router)
        {
            Router = router;
        }

        public void RegisterHandlers(IHandlerLoader loader)
        {

        }

        public void Start()
        {
            Binder.Start();
            Console.WriteLine("Started");
            _started = true;

            while (_started)
            {
                Thread.Sleep(5000);
            }
        }

        public void Stop()
        {
            Binder.Stop();
            _started = false;
            Console.WriteLine("Stoped");
        }

        private void ProcessRequest(object contextobj)
        {
            var context = (HttpListenerContext)contextobj;
            try
            {
                if (context.Request.HttpMethod.Equals("GET"))
                {
                    var msg = context.Request.HttpMethod + " " + context.Request.Url;

                    var runner = Router.Route(context.Request.Url.LocalPath);
                    var view = runner.Run();

                    var response = ViewBinder.BindView(view);

                    var b = Encoding.UTF8.GetBytes(response);
                    context.Response.ContentLength64 = b.Length;
                    context.Response.OutputStream.Write(b, 0, b.Length);
                    context.Response.StatusCode = 200;
                }
                else
                {
                    throw new Exception(context.Request.HttpMethod + " Not Implemented");
                }
            }
            catch (Exception ex)
            {
                //LOG ERRORS TO Console
                Console.WriteLine("Request error: " + ex);
                var exceptionView = new ExceptionView()
                                        {
                                            Exception = ex
                                        };

                var response = ViewBinder.BindView(exceptionView);

                byte[] b = Encoding.UTF8.GetBytes(response);
                context.Response.ContentLength64 = b.Length;
                context.Response.OutputStream.Write(b, 0, b.Length);
                context.Response.StatusCode = 500;
            }
            finally
            {
                context.Response.Close();
            }
        }
    }
}
