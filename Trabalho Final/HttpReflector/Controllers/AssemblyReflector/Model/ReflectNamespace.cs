using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Controllers.Model
{
    public class ReflectNamespace:ReflectModel
    {
        public Dictionary<string, ReflectType> Types { get; set; }
    }
}
