using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCustomEnumerable
{
    public interface ISortedEnumerable<T>: IEnumerable<T>
    {
        
    }

    public class SortedEnumerable<T>: ISortedEnumerable<T>
    {
        public IEnumerable<T> Sequence { get; set; }
        public Comparison<T> Criteria { get; set; }

        public SortedEnumerable(IEnumerable<T> seq, Comparison<T> comp )
        {
            this.Sequence = seq;
            this.Criteria = comp;
        }


        public IEnumerator<T> GetEnumerator()
        {
            var a = new List<T>(Sequence);
            a.Sort(Criteria);
            return a.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

    static class Util
    {
        public static IEnumerable<T> RemoveRepeated<T>(this IEnumerable<T> seq)
        {
            var set = new HashSet<T>();

            foreach (var item in seq)
            {
                if (set.Contains(item))
                {
                    continue;
                }
                else
                {
                    set.Add(item);
                    yield return item;
                }
            }
        }


        public static ISortedEnumerable<T> OrderBy<T, U>(this IEnumerable<T> seq, Func<T, U> criterium)
                where U : IComparable<U>
        {
            return new SortedEnumerable<T>(seq, new Comparison<T>((t1, t2) => criterium(t1).CompareTo(criterium(t2))));
        }

        public static ISortedEnumerable<T> ThenBy<T, U>(this ISortedEnumerable<T> seq, Func<T, U> criterium)
                where U : IComparable<U>
        {
            SortedEnumerable<T> s = (SortedEnumerable<T>) seq;
            return new SortedEnumerable<T>(seq,
                                           (t1, t2) =>
                                               {
                                                   int res = s.Criteria(t1, t2);
                                                   if (res != 0)
                                                       return res;
                                                   else
                                                       return criterium(t1).CompareTo(criterium(t2));
                                               }

                );
        }
    }
}
