using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using HttpReflector.Contracts.View;
using HttpReflector.Views.Attributes;

namespace HttpReflector.Views
{
    public static class ViewBinder
    {
        public static string RootFolder { get; set; }

        public static string BindView(IView view)
        {
            var viewType = view.GetType();
            TemplateView tviewAtt = null;
            foreach(var att in viewType.GetCustomAttributes(false))
            {
                if (att.GetType().Equals(typeof(TemplateView)))
                {
                    tviewAtt = (TemplateView)att;
                }
            }
            if(tviewAtt == null)
                throw new Exception();


            if(!File.Exists(RootFolder + tviewAtt.TemplateFile))
                throw new Exception();

            var sb = new StringBuilder();

            using (TextReader reader = File.OpenText(RootFolder + tviewAtt.TemplateFile))
            {
                sb.Append(reader.ReadToEnd());
            }

            var matcher = new Regex(@"<@([\.a-zA-Z]+)>");
            var match = matcher.Match(sb.ToString());

            while (match.Success)
            {
                string listProperties = match.Groups[1].Value;

                var propCallStack = new Stack<string>(listProperties.Split('.'));
                

                object value = view;
                foreach (var prop in propCallStack.Reverse())
                {
                    var pinfo = value.GetType().GetProperty(prop);

                    CollectionView cview = null;
 
                    foreach (var pinfoAttribute in pinfo.GetCustomAttributes(false))
                    {
                        if (pinfoAttribute.GetType().Equals(typeof(CollectionView))
                            && prop == pinfo.Name) 
                        {
                            cview = (CollectionView)pinfoAttribute;
                            break;
                        }
                    }

                    if (cview != null)
                    {
                        // the enumerable
                        value = pinfo.GetValue(value, new object[] { });

                        //TODO: Load from Property Text Descripton
                        var listsb = new StringBuilder();

                        foreach (var item in (IEnumerable)value)
                        {
                            //TODO: Need to load propper data
                            listsb.AppendFormat("{0} - {1}", item.GetType(), item);
                            listsb.AppendLine();
                        }

                        value = listsb.ToString();
                    }
                    else
                    {
                         value = pinfo.GetValue(value, new object[] { });
                    }

                }

                sb.Replace(match.Value, value.ToString());

                match = match.NextMatch();
            }

            
            return sb.ToString();
        }
    }
}
