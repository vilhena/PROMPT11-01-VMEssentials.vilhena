using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Controllers.Model
{
    public class ReflectMethod:ReflectModel
    {
        public List<ReflectParameter> Parameters { get; set; }

        public ReflectType Return { get; set; }
    }
}
