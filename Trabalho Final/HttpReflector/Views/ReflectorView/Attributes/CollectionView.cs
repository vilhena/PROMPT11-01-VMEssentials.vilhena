using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CollectionView: Attribute
    {
        public string TemplateFile { get; set; }

        public CollectionView(string templateFile)
        {
            TemplateFile = templateFile;
        }
    }
}
