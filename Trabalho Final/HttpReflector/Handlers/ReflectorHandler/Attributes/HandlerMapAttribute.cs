using System;

namespace HttpReflector.Handlers.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class HandlerMapAttribute : Attribute
    {
        public string Pattern { get; set; }
        public HandlerMapAttribute(string pattern)
        {
            Pattern = pattern;
        }
    }
}
