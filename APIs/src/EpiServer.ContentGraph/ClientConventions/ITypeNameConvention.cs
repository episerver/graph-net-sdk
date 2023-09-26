using System;

namespace EPiServer.Find.ClientConventions
{
    public interface ITypeNameConvention
    {
        string GetTypeName(Type type);
    }
}
