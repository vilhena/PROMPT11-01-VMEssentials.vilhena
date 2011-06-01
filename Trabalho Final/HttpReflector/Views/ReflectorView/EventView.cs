using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Contracts.View;
using HttpReflector.Controllers.Model;

namespace HttpReflector.Views
{
    public class EventView : IView
    {
        public ReflectType Type { get; set; }
        public ReflectEvent Event { get; set; }
    }
}
