using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Binding
{
    internal abstract class MemberBinder
    {
        private readonly Type _memberType;

        protected MemberBinder(MemberInfo memberInfo, Type memberType)
        {
            ThrowIfMemberTypeIsInvalid(memberInfo, memberType);
            _memberType = memberType;
        }
        private static void ThrowIfMemberTypeIsInvalid(MemberInfo memberInfo, Type memberType)
        {
            if (memberType.IsPrimitive || memberType == typeof(string)) return;
            throw new NotPrimitiveMemberBinderException(memberInfo, memberType);
        }

        public void Bind(object obj, string value)
        {
            var convertedValue = Convert.ChangeType(value, _memberType);
            DoSetValue(obj, convertedValue);
        }

        protected abstract void DoSetValue(object obj, object value);
    }

    class FieldBinder : MemberBinder
    {
        private readonly FieldInfo _fi;

        public FieldBinder(FieldInfo fi)
            : base(fi, fi.FieldType)
        {
            _fi = fi;
        }

        protected override void DoSetValue(object obj, object value)
        {
            _fi.SetValue(obj, value);
        }
    }

    class PropBinder : MemberBinder
    {
        private readonly PropertyInfo _pi;

        public PropBinder(PropertyInfo pi)
            : base(pi, pi.PropertyType)
        {
            _pi = pi;
        }

        protected override void DoSetValue(object obj, object value)
        {
            _pi.SetValue(obj, value, null);
        }
    }
}
