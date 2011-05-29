using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Utils.Exception
{
    public class KeyNotFoundTreeException<TKey>:TreeException
    {
        public TKey Key { get; set; }

        public KeyNotFoundTreeException(TKey key)
            :base(string.Format("Key {0} does not exists",
            key))
        {
            this.Key = key;
        }
    }
}
