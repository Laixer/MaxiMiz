using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Poller.Extensions
{
    public static class EnumExtensions
    {
        public static string GetEnumMemberName<TEnum>(this TEnum enumerationValue)
            where TEnum : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(EnumMemberAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((EnumMemberAttribute)attrs[0]).Value;
                }
            }
            return enumerationValue.ToString();
        }
    }
}
