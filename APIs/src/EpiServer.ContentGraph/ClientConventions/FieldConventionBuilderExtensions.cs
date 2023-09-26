using EPiServer.Find.Helpers;
using EPiServer.Find.Helpers.Text;

namespace EPiServer.Find.ClientConventions
{
    public static class FieldConventionBuilderExtensions
    {
        public static FieldConventionBuilder<string> StripHtml(this FieldConventionBuilder<string> conventionBuilder)
        {
            conventionBuilder.ConvertBeforeSerializing(x => x.IsNotNull() ? x.StripHtml() : null);
            return conventionBuilder;
        }
    }
}