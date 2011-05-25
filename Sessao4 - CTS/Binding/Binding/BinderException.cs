using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Binding
{
    public abstract class BinderException : Exception
    {
        protected BinderException(string msg) : base(msg) { }
    }

    public class NotPrimitiveMemberBinderException : BinderException
    {
        private Type memberType;

        public MemberInfo MemberInfo { get; private set; }

        public NotPrimitiveMemberBinderException(MemberInfo mi)
            :base(string.Format("Member {0} of type [1] is not primitive",
            mi.Name, mi.DeclaringType.Name))
        {
            this.MemberInfo = mi;
        }

        public NotPrimitiveMemberBinderException(MemberInfo memberInfo, Type memberType)
            : this(memberInfo)
        {
            this.MemberInfo = memberInfo;
            this.memberType = memberType;
        }
    }

    public class InexistentMemberBinderException : BinderException
    {
        public string MemberName { get; set; }

        public InexistentMemberBinderException(string field)
            :base(string.Format("Member {0} does not exists",
            field))
        {
            this.MemberName = field;
        }
    }

}
