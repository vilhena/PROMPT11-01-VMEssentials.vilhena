using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Views.Attributes
{
    public class PropertyView: Attribute
    {
        public string Name { get; set; }
        public string PropertyName { get; set; }
    }
}
