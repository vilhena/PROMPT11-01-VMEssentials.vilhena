using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Utils
{
    public class Node<TKey,TValue>: INode<TKey,TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public INode<TKey, TValue> Parent { get; set; }
        public SortedList<TKey, INode<TKey, TValue>> Children { get; set; }
    }
}
