using System;
using System.Reflection;

namespace EPiServer.ContentGraph.Helpers.Reflection
{
    public static class MemberInfoExtensions
    {
        public static Type GetMemberReturnType(this MemberInfo member)
        {
            if (member is PropertyInfo)
            {
                return ((PropertyInfo)member).PropertyType;
            }
            if (member is MethodInfo)
            {
                return ((MethodInfo)member).ReturnType;
            }

            return ((FieldInfo)member).FieldType;
        }
    }
}
