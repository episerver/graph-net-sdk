using System.Globalization;

namespace EPiServer.ContentGraph.Helpers
{
    public static class NumberExtension
    {
        public static string ToInvariantString(this double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
        public static string ToInvariantString(this float value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
