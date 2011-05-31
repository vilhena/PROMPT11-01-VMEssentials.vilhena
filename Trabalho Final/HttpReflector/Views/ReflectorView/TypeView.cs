using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Contracts.View;
using HttpReflector.Controllers.Model;
using HttpReflector.Views.Attributes;

namespace HttpReflector.Views
{
    //TODO: use by default same name of class plus html
    [TemplateView("TypeView.txt")]
    public class TypeView: IView
    {

        public ReflectType Type { get; set; }

        [CollectionView("TypeView.Method.txt")]
        public List<ReflectMethod> Methods { get; set; }

        [CollectionView("TypeView.Constructor.txt")]
        public List<ReflectMethod> Constructors { get; set; }

        [CollectionView("TypeView.Field.txt")]
        public List<ReflectField> Fields { get; set; }

        [CollectionView("TypeView.Property.txt")]
        public List<ReflectProperty> Properties { get; set; }

        [CollectionView("TypeView.Event.txt")]
        public List<ReflectEvent> Events { get; set; }
    }
}
