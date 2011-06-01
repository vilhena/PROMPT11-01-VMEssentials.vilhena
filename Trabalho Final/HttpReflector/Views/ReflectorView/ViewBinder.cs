using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using HttpReflector.Contracts.View;
using HttpReflector.Views.Exceptions;

namespace HttpReflector.Views
{
    public static class ViewBinder
    {
        public static string RootFolder { get; set; }

        public static string BindView(IView view)
        {
            var viewType = view.GetType();
            if (!File.Exists(RootFolder + viewType.Name + ".txt"))
                throw new TemplateViewNotFoundViewException(viewType.Name, RootFolder + viewType.Name + ".txt");

            return MatchViewRootProperties(view, MatchViewProperties(view, viewType.Name));

        }

        private static string MatchViewRootProperties(object view, string output)
        {
            var sb = new StringBuilder(output);
            // Normal access properties
            var matcher = new Regex(@"<%([\.a-zA-Z]+)>");
            var match = matcher.Match(sb.ToString());

            while (match.Success)
            {
                var listProperties = match.Groups[1].Value;
                var propCallStack = new Stack<string>(listProperties.Split('.'));

                var value = view;
                foreach (var prop in propCallStack.Reverse())
                {
                    var pinfo = value.GetType().GetProperty(prop);
                    //Try to Map invalid Data
                    if (pinfo == null)
                        throw new InvalidDataBindViewException(listProperties, prop, value.GetType().Name);
                    value = pinfo.GetValue(value, new object[] { });
                }
                //MustEscapeData
                sb.Replace(match.Value, value.ToString());
                match = match.NextMatch();
            }
            return sb.ToString();
        }

        private static string MatchViewProperties(object view, string filePath)
        {
            if (!File.Exists(RootFolder + filePath + ".txt"))
                throw new TemplateViewNotFoundViewException(
                    view.GetType().Name, RootFolder + filePath + ".txt");

            var sb = new StringBuilder();

            using (TextReader reader = File.OpenText(RootFolder + filePath + ".txt"))
            {
                sb.Append(reader.ReadToEnd());
            }

            // Normal access properties
            var matcher = new Regex(@"<\$([\.a-zA-Z]+)>");
            var match = matcher.Match(sb.ToString());

            while (match.Success)
            {
                var listProperties = match.Groups[1].Value;
                var propCallStack = new Stack<string>(listProperties.Split('.'));
                
                var value = view;
                foreach (var prop in propCallStack.Reverse())
                {
                    var pinfo = value.GetType().GetProperty(prop);
                    //Try to Map invalid Data
                    if (pinfo == null)
                        throw new InvalidDataBindViewException(listProperties, prop, value.GetType().Name);
                    value = pinfo.GetValue(value, new object[] { });
                }
                //MustEscapeData
                sb.Replace(match.Value, value.ToString());
                match = match.NextMatch();
            }


            matcher = new Regex(@"<@([\.a-zA-Z]+)>");
            var matchList = matcher.Match(sb.ToString());

            while (matchList.Success)
            {
                var listProperties = matchList.Groups[1].Value;
                var propCallStack = new Stack<string>(listProperties.Split('.'));

                var value = view;
                var propIndex = propCallStack.Count;
                foreach (var prop in propCallStack.Reverse())
                {
                    --propIndex;
                    var pinfo = value.GetType().GetProperty(prop);

                    //Try to Map invalid Data
                    if (pinfo == null)
                        throw new InvalidDataBindViewException(listProperties, prop, value.GetType().Name);

                    value = pinfo.GetValue(value, new object[] { });

                    //Is enumerable and it's the last callstack it can be usefull to use fom .Count on collections
                    if (value is IEnumerable && propIndex == 0)
                    {
                        // the enumerable
                        var listsb = new StringBuilder();
                        foreach (var item in (IEnumerable)value)
                        {
                            listsb.Append(MatchViewProperties(item, filePath + "." + pinfo.Name));
                        }

                        value = listsb.ToString();
                    }
                }
                //MustEscapeData
                sb.Replace(matchList.Value, value.ToString());

                matchList = matchList.NextMatch();
            }
            return sb.ToString();
        }
    }
}
