﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Contracts.View;
using HttpReflector.Controllers.Model;

namespace HttpReflector.Views
{
    public class FieldView : IView
    {
        public ReflectType Type { get; set; }
        public ReflectField Field { get; set; }
    }
}
