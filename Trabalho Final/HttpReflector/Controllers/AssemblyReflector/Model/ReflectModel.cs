﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Controllers.Model
{
    public enum ReflectVisibility
    {
        Public ,
        Protected,
        Private,
    }
    public abstract class ReflectModel
    {
        public string Name { get; set; }
    }
}
