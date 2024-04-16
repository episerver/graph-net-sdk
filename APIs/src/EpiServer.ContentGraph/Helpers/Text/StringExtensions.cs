using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace EPiServer.ContentGraph.Helpers.Text
{
    public static class StringExtensions
    {
        private static HashSet<string> nonWordBreakingElements = new HashSet<string>(makeNonWordBreakingElementsList(), StringComparer.OrdinalIgnoreCase);

        private static string[] makeNonWordBreakingElementsList()
        {
            return "a,abbr,acronym,b,bdo,big,cite,code,dfn,em,i,kbd,samp,small,span,strong,sub,sup,tt,var".Split(',');
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNotNullOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrEmpty(value) || value.Trim().Length == 0;
        }

        public static bool IsAllLowerCase(this string value)
        {
            foreach (char character in value)
            {
                if (char.IsUpper(character))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool ContainsWhiteSpace(this string value)
        {
            foreach (char character in value)
            {
                if (char.IsWhiteSpace(character))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool In(this string value, IEnumerable<string> list)
        {
            return list.Any(x => x.Equals(value));
        }

        public static bool In(this string value, params string[] strings)
        {
            return strings.Any(x => x.Equals(value));
        }

        public static string Concatenate(this IEnumerable<string> strings, string delimiter)
        {
            if (!strings.Any())
            {
                return string.Empty;
            }
            return strings.Aggregate((accumulate, param) => accumulate + delimiter + param);
        }

        /// <summary>
        /// Strips html tags from text and decodes any encoded characters.
        /// This ensures that text is indexed the same way that a user reads it
        /// in a browser, and enhances search over these words.
        /// </summary>
        /// <param name="htmlText">Text from which to strip html tags and decode encoded characters</param>
        /// <returns>Decoded text free from html tags</returns>
        public static string StripHtml(this string htmlText)
        {
            if (htmlText.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var strippedText = Regex.Replace(Regex.Replace(htmlText, "</?(\\w+)[^>]*?>", (match) =>
            {
                return nonWordBreakingElements.Contains(match.Groups[1].Value) ? string.Empty : " ";
            }).Trim(), "\\s+", " ");

            return HttpUtility.HtmlDecode(strippedText);
        }

        public static string AppendQueryStringParameter(this string url, string key, string value)
        {
            if (url.Contains("?"))
            {
                return string.Format("{0}&{1}={2}", url, key, value);
            }

            return string.Format("{0}?{1}={2}", url, key, value);
        }

        public static bool TryParse(this string value, out Guid guidValue)
        {
            try
            {
                guidValue = new Guid(value);
                return true;
            }
            catch (Exception)
            {
                guidValue = Guid.Empty;
                return false;
            }
        }

        public static string AddFallbackToBeginningOfHighlightedText(
            this string highlightedText, 
            string fallbackText, 
            int maxLength, 
            int numberOfWordsToCompareForSameTextCheck = 3, 
            string separator = " ... ")
        {
            separator = separator ?? " ";
            highlightedText = highlightedText ?? string.Empty;
            fallbackText = fallbackText ?? string.Empty;
            var highlightedTextExcludingMarkup = highlightedText.StripHtml();
            var highlightedTextExcludingMarkupLength = highlightedTextExcludingMarkup.Length;
            if (highlightedTextExcludingMarkupLength >= maxLength)
            {
                return highlightedText;
            }

            if (highlightedTextExcludingMarkup.Length > 0)
            {
                var highlightedTextExcludingMarkupAndTrailingPeriods = highlightedTextExcludingMarkup.TrimEnd('.');

                var indexOfHighlightedTextInFallback = fallbackText.IndexOf(highlightedTextExcludingMarkupAndTrailingPeriods,
                                                                            StringComparison.InvariantCulture);
                if (indexOfHighlightedTextInFallback > -1)
                {
                    fallbackText = fallbackText.Substring(0, indexOfHighlightedTextInFallback);
                }
            }

            if (fallbackText.Length == 0)
            {
                return highlightedText;
            }

            var fallbackLines = Array.ConvertAll(fallbackText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries), p => p.Trim());
            fallbackText = string.Join(" ", fallbackLines);

            if (numberOfWordsToCompareForSameTextCheck > 0 && 
                fallbackText.Split(' ')
                .Take(numberOfWordsToCompareForSameTextCheck)
                .SequenceEqual(
                    highlightedTextExcludingMarkup.Split(' ').Take(numberOfWordsToCompareForSameTextCheck)))
            {
                return highlightedText;
            }
            var fallbackLength = maxLength - highlightedTextExcludingMarkupLength - separator.Length;
            if (fallbackLength < 1)
            {
                return highlightedText;
            }
            if (fallbackText.Length < fallbackLength)
            {
                fallbackLength = fallbackText.Length;
            }
            if (highlightedTextExcludingMarkupLength == 0)
            {
                separator = string.Empty;
            }
            return fallbackText.TruncateWholeWords(fallbackLength).TrimEnd() + separator + highlightedText;

        }

        public static string TruncateWholeWords(this string value, int maxLength)
        {
            if(value == null)
            {
                return string.Empty;
            }
            if (value.Length <= maxLength)
            {
                return value;
            }
            var indexOfFirstSpace = value.IndexOf(" ", StringComparison.InvariantCultureIgnoreCase);
            if (indexOfFirstSpace == -1)
            {
                return value.Substring(0, maxLength);
            }
            var indexOfFirstSpaceAfterMaxLength 
                = value.IndexOf(" ", maxLength, StringComparison.InvariantCultureIgnoreCase) ;
            if (indexOfFirstSpaceAfterMaxLength == -1)
            {
                var indexOfLastSpace = value.LastIndexOf(" ", StringComparison.InvariantCultureIgnoreCase);
                return value.Substring(0, indexOfLastSpace);
            }
            return value.Substring(0, indexOfFirstSpaceAfterMaxLength);
        }

        public static void ValidateNotNullOrEmptyArgument(this string argument, string paramName)
        {
            if (argument.IsNullOrEmpty())
            {
                StackTrace stackTrace = new StackTrace();
                var method = stackTrace.GetFrame(1).GetMethod();
                var message =
                    string.Format(
                        "The method {0} in class {1} has been invoked with null/empty as value for the argument {2}.",
                        method.Name,
                        method.DeclaringType,
                        paramName);
                throw new ArgumentNullException(paramName, message);
            }
        }

        public static string MD5Hash(this string value)
        {
            if (value.IsNull())
            {
                return null;
            }

            StringBuilder hash = new();
            MD5CryptoServiceProvider md5provider = new();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(value));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        public static bool IsWildcardPrefix(this string value)
        {
            if (IsNullOrWhiteSpace(value))
            {
                return false;
            }
                            
            if ( value.StartsWith("*") && value.Length > 1)
            {
                return true;
            }

            if(value.StartsWith("?"))
            {
                return true;
            }

            return false;
        }
        public static bool IsValidName(this string name, int length=25)
        {
            Regex reg = new Regex(@"^[a-zA-Z_]\w*$");
            return reg.IsMatch(name) && name.Length <= length;
        }
    }
}
