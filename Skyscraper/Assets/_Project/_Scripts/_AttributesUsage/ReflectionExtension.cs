using System;
using System.Reflection;

public static class ReflectionExtension
{
    public static bool IsDefined<T>(this MemberInfo type) where T : Attribute
    {
        return type.IsDefined(typeof(T));
    }

    public static bool IsDefined<T>(this MemberInfo type, bool inherit) where T : Attribute
    {
        return type.IsDefined(typeof(T), inherit);
    }
}