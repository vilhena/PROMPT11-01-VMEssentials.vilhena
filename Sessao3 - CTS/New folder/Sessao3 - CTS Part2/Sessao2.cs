using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sessao2
{

    public class MyEventClass
    {
        public string eventName;

        public MyEventClass(string name)
        {
            eventName = name;
        }


        public void MyEvent(object sender, EventArgs args)
        {
            //controlo gerador, o nome do evento gerado, a data/hora em que foi gerado e o argumento associado.

            Console.WriteLine("Controlo={0} || evento={1} || data={2} || argumento={3}"
                , ((Control)sender).Name, eventName, DateTime.Now, args.GetType().Name);
        }
    }

    public class SessionRecorder
    {
        static Form theOriginalForm;
        static Dictionary<string, MyEventClass> dic = new Dictionary<string, MyEventClass>();
        // Recebe na construção o Form de que se pretende gravar os
        // eventos gerados durante um período de utilização
        public SessionRecorder(Form form)
        {
            theOriginalForm = form;
            
        }
        // Inicia o período de gravação de eventos
        public void StartRecorder()
        {
            foreach (Control control in theOriginalForm.Controls)
            {
                foreach (var evento in control.GetType().GetEvents())
                {
                    Type t = evento.EventHandlerType;

                    MyEventClass x = new MyEventClass(evento.Name);


                    var minfo = x.GetType().GetMethod("MyEvent");
                    if (minfo != null)
                    {

                        if (dic.ContainsKey(evento.EventHandlerType.Name))
                        {
                            Delegate toAdd = Delegate.CreateDelegate(evento.EventHandlerType, dic[evento.EventHandlerType.Name],
                                dic[evento.EventHandlerType.Name].GetType().GetMethod("MyEvent"));
                            evento.AddEventHandler(control, toAdd);
                        }
                        else
                        {
                            Delegate toAdd = Delegate.CreateDelegate(evento.EventHandlerType, x, minfo);
                            evento.AddEventHandler(control, toAdd);
                            dic.Add(evento.EventHandlerType.Name, x);
                        }
                    }

                    //evento.AddEventHandler(evento,
                    //evento.GetAddMethod().Invoke(control,new object[]{new EventArgs()});

                }
            }
        }
        // Termina o período de gravação de eventos
        public void StopRecorder()
        {

            //remove all

            foreach (Control control in theOriginalForm.Controls)
            {
                foreach (var evento in control.GetType().GetEvents())
                {
                    if (dic.ContainsKey(evento.EventHandlerType.Name))
                    {
                        Delegate toDelete = Delegate.CreateDelegate(evento.EventHandlerType, dic[evento.EventHandlerType.Name],
                                dic[evento.EventHandlerType.Name].GetType().GetMethod("MyEvent"));
                        evento.RemoveEventHandler(control, toDelete);
                    }
                }
            }
        }


        // Guarda a informação sobre os eventos no ficheiro fileName
        public void SaveEvents(string fileName)
        {
            Console.WriteLine(fileName);
        }



    }
}
