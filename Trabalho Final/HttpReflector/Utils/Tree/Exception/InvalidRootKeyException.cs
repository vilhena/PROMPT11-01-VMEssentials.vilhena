using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Utils.Exception
{
    class InvalidRootKeyException<TKey>:TreeException
    {
        public TKey Key { get; set; }

        public InvalidRootKeyException(TKey key)
            :base(string.Format("Root key {0} does not exists",
            key))
        {
            this.Key = key;
        }
    }
}
