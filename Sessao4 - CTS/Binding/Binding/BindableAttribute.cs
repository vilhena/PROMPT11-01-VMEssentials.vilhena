using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Binding
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class BindingAttribute : Attribute
    {
        public string Name { get; set; }
        public bool Required { get; set; }
    }
}
