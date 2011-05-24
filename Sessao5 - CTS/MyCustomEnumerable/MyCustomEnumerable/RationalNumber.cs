using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCustomEnumerable
{
    public struct RationalNumber:  IComparable<RationalNumber>, IEquatable<RationalNumber>
    {
        public readonly int _numerator, _denominator;



        public RationalNumber(int numerator, int denominator)
        {
            if (denominator == 0) throw new InvalidRationalException();
            _numerator = numerator;
            _denominator = denominator;
        }
        public static RationalNumber operator +(RationalNumber n1, RationalNumber n2)
        {
            int nn, nd;
            if (n1._denominator == n2._denominator)
            {
                nn = n1._numerator + n2._numerator;
                nd = n1._denominator;
            }
            else
            {
                nn = n1._numerator * n2._denominator + n2._numerator * n1._denominator;
                nd = n1._denominator * n2._denominator;
            }
            return new RationalNumber(nn, nd);
        }


        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(_numerator);
            sb.Append('/');
            sb.Append(_denominator);
            return sb.ToString();
        }


        public int CompareTo(RationalNumber other)
        {
            return (this._denominator - other._denominator) + (this._numerator - other._numerator);
        }



        public bool Equals(RationalNumber other)
        {
            return (this._denominator == other._denominator) && (this._numerator == other._numerator);
        }

        
    }
}
