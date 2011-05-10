using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace MyReflect
{

    public class ObjectComparer : IEqualityComparer<object>
    {
        public int GetHashCode(object bx)
        {
            if(bx.GetType() == typeof(DirectoryInfo))
                return ((DirectoryInfo) bx).FullName.GetHashCode();
            else
                return bx.GetHashCode();
        }

        public bool Equals(object b1, object b2)
        {
            if (b1.GetType() == typeof(DirectoryInfo) && b2.GetType() == typeof(DirectoryInfo))
                return ((DirectoryInfo)b1).FullName == ((DirectoryInfo)b2).FullName;
            else
                return b1.Equals(b2);
        }
    }


    public class PID
    {
        public static Dictionary<object, string> processed = new Dictionary<object, string>(new ObjectComparer());

        public static void Inspect(object obj)
        {

            foreach (var property in obj.GetType().GetProperties())
            {
                var value = property.GetValue(obj, new object[0]);
                Console.WriteLine("{0} - {1}", property.Name, value == null ? String.Empty : value);
            }
        }

        public static void InspectToFile(object obj)
        {
            var filename = obj.GetType().Name;

            var sbuild = new StreamWriter(filename + ".html");

            sbuild.WriteLine("<html>");
            sbuild.WriteLine("<body>");
            sbuild.WriteLine(@"<table border=1>");
            foreach (var property in obj.GetType().GetProperties())
            {
                sbuild.WriteLine("<tr>");

                var value = property.GetValue(obj, new object[0]);
                sbuild.WriteLine("<td>{0}</td><td>{1}</td>", property.Name, value == null ? String.Empty : value);

                sbuild.WriteLine("</tr>");
            }
            sbuild.WriteLine("</table>");
            sbuild.WriteLine("</body>");
            sbuild.WriteLine("</html>");

            sbuild.Close();
        }

        public static void SeekList(object obj, string path)
        {

            return;
        }


        public static string Seek(object obj, string path)
        {
            var filename = path + "_" + obj.GetType().Name;

            processed.Add(obj, filename);

            var sbuild = new StreamWriter(@".\teste\" + filename + ".html");

            sbuild.WriteLine("<html>");
            sbuild.WriteLine("<body>");
            sbuild.WriteLine("<h1>{0}<h1/>",obj.GetType().Name);
            sbuild.WriteLine(@"<table border=1>");
            foreach (var property in obj.GetType().GetProperties())
            {
                if (property.GetIndexParameters().Length == 0)
                {

                    sbuild.WriteLine("<tr>");

                    var value = property.GetValue(obj, new object[0]);


                    if (property.PropertyType.IsPrimitive || value == null || property.PropertyType == typeof(String))
                    {

                        sbuild.WriteLine("<td>{0}</td><td>{1}</td>", property.Name, value == null ? String.Empty : value);
                    }
                    else
                    {
                        if (!processed.ContainsKey(value))
                        {
                            string processedfilename = Seek(value, filename);
                            sbuild.WriteLine("<td>{0}</td><td><a href='{1}.html'>{2}</a></td>", property.Name, processedfilename, value.ToString());
                            
                        }
                        else
                        {
                            sbuild.WriteLine("<td>{0}</td><td><a href='{1}.html'>{2}</a></td>", property.Name, processed[value], value.ToString());
                        }
                    }
                    
                    sbuild.WriteLine("</tr>");
                }
            }

            if (obj.GetType().GetInterfaces().Contains(typeof(IEnumerable)))
            {
                SeekList(obj, path);
            }

            sbuild.WriteLine("</table>");
            sbuild.WriteLine("</body>");
            sbuild.WriteLine("</html>");

            sbuild.Close();

            return filename;
        }
    }
}
