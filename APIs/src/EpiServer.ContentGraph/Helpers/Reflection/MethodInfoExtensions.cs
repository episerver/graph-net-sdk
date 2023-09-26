using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace EPiServer.ContentGraph.Helpers.Reflection
{
    public static class MethodInfoExtensions
    {
        public static bool IsExtensionMethod(this MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttributes(typeof (ExtensionAttribute), false).Length > 0;
        }

        public static bool HasGenericTypeDefinition(this MethodInfo methodInfo, Type genericType)
        {
            return methodInfo.ReturnType.IsGenericType
                   && genericType.IsAssignableFrom(methodInfo.ReturnType.GetGenericTypeDefinition());
        }
    }
}
