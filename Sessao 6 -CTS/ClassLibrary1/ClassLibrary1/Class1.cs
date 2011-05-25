using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;

namespace ClassLibrary1
{
    public class Class1
    {
        public static void Main()
        {
            using (var listner = new HttpListener())
            {
                listner.Prefixes.Add("http://localhost:8080/");
                listner.Start();
                while (true)
                {
                    var ctx = listner.GetContext();
                    Console.WriteLine("ContextReceived" + ctx.Request.Url);
                    Console.WriteLine("teste" + ctx.Request.HttpMethod);
                    var w = new StreamWriter(ctx.Response.OutputStream);
                    w.WriteLine();
                    ctx.Response.StatusCode = 500;
                    ctx.Response.Close();
                }
            }
        }

        //forma declarativa de escrever html
        // table(
        // tr(th("datos1"),td("dados"))
        //

        // capacidade de testes unitarios

    }
}
