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

            //TODO: CHANGE THIS
            while (true)
            {
                Thread.Sleep(1000);
            }
        }

        public void Stop()
        {
            Binder.Stop();
        }

        private void ProcessRequest(object contextobj)
        {
            try
            {
                var context = (HttpListenerContext)contextobj;

                //context.Request.Url.GetComponents()

                if (context.Request.HttpMethod.Equals("GET"))
                {
                    var msg = context.Request.HttpMethod + " " + context.Request.Url;

                    IHandler runner = Router.Route(context.Request.Url.LocalPath);
                    var view = runner.Run();
                    
                    var response = ViewBinder.BindView(view);

                    byte[] b = Encoding.UTF8.GetBytes(response);
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
