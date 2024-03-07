using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPiServer.ContentGraph.Helpers
{
    public static class EnumExtensions
    {
        public static string ToLowerCaseUnderscoreString(this Enum value)
        {
            var stringValue = new StringBuilder();
            bool first = true;
            value.ToString().ToList().ForEach(x =>
            {
                if (char.IsUpper(x))
                {
                    if (!first)
                    {
                        stringValue.Append('_');
                    }
                    else
                    {
                        first = false;
                    }
                    stringValue.Append(x.ToString().ToLowerInvariant());
                }
                else
                {
                    stringValue.Append(x);
                }
            });

            return stringValue.ToString();
        }
    }
}
