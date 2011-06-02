using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpReflector.Utils.Exception;

namespace HttpReflector.Utils
{
    public class Tree<TKey, TValue>:INode<TKey,TValue>
    {
        private INode<TKey, TValue> _root;


        public Tree()
        {
            _root = new Node<TKey, TValue>()
            {
                Children = new SortedList<TKey, INode<TKey, TValue>>(),
                Key = default(TKey),
                Value = default(TValue),
                Parent = null
            };
        }

        private INode<TKey, TValue> FindNode(IEnumerable<TKey> keyList, bool throws)
        {
            var currentNode = _root;
            foreach (var key in keyList)
            {

                if (!currentNode.Children.ContainsKey(key))
                {
                    if (throws)
                        throw new KeyNotFoundTreeException<TKey>(key);
                    return null;
                }

                currentNode = currentNode.Children[key];
            }
            return currentNode;
        }


        public TValue GetValue(IEnumerable<TKey> keyList)
        {
            return FindNode(keyList, true).Value;
        }

        private IEnumerable<TValue> GetChildrenValues(INode<TKey,TValue> node)
        {
            var valueList = new List<TValue>();
            foreach (var child in node.Children)
            {
                if (child.Value.Children == null || child.Value.Children.Count == 0)
                {
                    valueList.Add(child.Value.Value);
                }
                else
                {
                    valueList.Add(child.Value.Value);
                    valueList.AddRange(GetChildrenValues(child.Value));
                }
            }
            return valueList;
        }

        public IEnumerable<TValue> GetAllChildrenValues(IEnumerable<TKey> keyList)
        {
            var valueList = new List<TValue>();
            var node = FindNode(keyList, true);

            return GetChildrenValues(node);
        }

        public void SetValue(IEnumerable<TKey> keyList, TValue value)
        {
            FindNode(keyList, true).Value = value;
        }

        public bool ContainsKey(IEnumerable<TKey> keyList)
        {
            return FindNode(keyList,false) != null;
        }

        public void Add(IEnumerable<TKey> keyList, TValue value)
        {
            INode<TKey, TValue> currentNode = _root;
            foreach (var key in keyList)
            {
                if (!currentNode.Children.ContainsKey(key))
                    currentNode.Children.Add(key, new Node<TKey, TValue>()
                                                      {
                                                          Children = new SortedList<TKey, INode<TKey, TValue>>(),
                                                          Key = key,
                                                          Parent = currentNode,
                                                          Value = default(TValue)
                                                      });
                currentNode = currentNode.Children[key];
            }

            if (currentNode != null) 
                currentNode.Value = value;
        }

        public TKey Key
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public TValue Value
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public INode<TKey, TValue> Parent
        {
            get
            {
                return null;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public SortedList<TKey, INode<TKey, TValue>> Children
        {
            get
            {
                return _root.Children;
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public int Count
        {
            get { return _root.Children.Count; }
        }
    }
}
