using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TemplateView : Attribute
    {
        public string TemplateFile { get; set; }

        public TemplateView(string templateFile)
        {
            TemplateFile = templateFile;
        }
    }
}
