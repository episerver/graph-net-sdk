using System.Linq.Expressions;
using EPiServer.Find.Helpers.Reflection;

namespace EPiServer.Find.ClientConventions
{
    public class TypeSuffixFieldNameConvention : FieldNameConvention
    {
        public TypeSuffixFieldNameConvention() : this(null) { }

        public TypeSuffixFieldNameConvention(NestedConventions nestedConventions) : base(nestedConventions) { }

        public override string GetFieldName(Expression fieldSelector)
        {
            var fieldType = fieldSelector.GetReturnType();

            var name = base.GetFieldName(fieldSelector);

            return TypeSuffix.GetSuffixedFieldName(name, fieldType);
        }
    }
}
