using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCustomEnumerable
{
    class Program
    {
        static void Main(string[] args)
        {
            var rnumbers = new List<RationalNumber>
                                                {
                                                    new RationalNumber(1, 1),
                                                    new RationalNumber(2, 1),
                                                    new RationalNumber(1, 1),
                                                    new RationalNumber(2, 1),
                                                    new RationalNumber(1, 1),
                                                    new RationalNumber(1, 3),
                                                    new RationalNumber(1, 1),
                                                    new RationalNumber(2, 1),
                                                };

            //var n = rnumbers.Where(r => r.Equals(new RationalNumber(1, 1)));

            //rnumbers.ForEach(r => Console.WriteLine(r));

            //rnumbers.Sort();

            //Console.WriteLine("......2");

            //rnumbers.RemoveRepeated().ToList().ForEach(r => Console.WriteLine(r));

            Console.WriteLine("......3");

            rnumbers.OrderBy(s => s._numerator).ThenBy(s => s._denominator)
            .ToList().ForEach(r => Console.WriteLine(r));
        }
    }
}
