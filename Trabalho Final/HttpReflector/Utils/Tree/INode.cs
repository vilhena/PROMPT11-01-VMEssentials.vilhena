using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Utils
{
    public interface INode<TKey, TValue>
    {
        TKey Key { get; set; }
        TValue Value { get; set; }

        INode<TKey, TValue> Parent { get; set; }
        SortedList<TKey, INode<TKey, TValue>> Children { get; set; }
    }
}
