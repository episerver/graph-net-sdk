using System;
using EPiServer.Find.Helpers;

namespace EPiServer.Find.ClientConventions
{
    public class DefaultTypeNameConvention : ITypeNameConvention
    {
        public string GetTypeName(Type type)
        {
            type.ValidateNotNullArgument("type");
            while (type.Assembly.IsDynamic && type.BaseType is object)
            {
                type = type.BaseType;
            }

            return type.FullName.Replace('.', '_').Replace("+", "__");
        }
    }
}
