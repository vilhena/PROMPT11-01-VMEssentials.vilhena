using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Views.Attributes
{
    public class CollectionView: Attribute
    {
        public string TemplateFile { get; set; }
    }
}
