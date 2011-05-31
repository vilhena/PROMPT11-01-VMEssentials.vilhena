using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Handlers.Exceptions
{
    public class MapKeyNotFoundMapBinderException : Exception
    {
        public string ExpectedKey { get; set; }
        public MapKeyNotFoundMapBinderException(string key)
            : base(string.Format("Map key {0} does not exists",
            key))
        {
            this.ExpectedKey = key;
        }
    }
}
