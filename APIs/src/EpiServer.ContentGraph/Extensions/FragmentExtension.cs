using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.Helpers.Reflection;
using System.Linq.Expressions;

namespace EPiServer.ContentGraph.Extensions
{
    public static class FragmentExtension
    {
        public static DeligateRecursiveBuilder Recursive(this object property,  int depth)
        {
            return new DeligateRecursiveBuilder(field => new Recursion() { 
                FieldName = field, 
                RecursiveDepth = depth
            });
        }
    }
}
