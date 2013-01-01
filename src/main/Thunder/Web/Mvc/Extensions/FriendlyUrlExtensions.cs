using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thunder.Web.Mvc.Extensions
{
    /// <summary>
    /// Friendly url extensions
    /// </summary>
    public static class FriendlyUrlExtensions
    {
        private static string Clear(string text)
        {
            var dictionary = new Dictionary<char, char[]>
                                 {
                                     {'a', new [] {'à', 'á', 'ä', 'â', 'ã'}},
                                     {'c', new [] {'ç'}},
                                     {'e', new [] {'è', 'é', 'ë', 'ê'}},
                                     {'i', new [] {'ì', 'í', 'ï', 'î'}},
                                     {'o', new [] {'ò', 'ó', 'ö', 'ô', 'õ'}},
                                     {'u', new [] {'ù', 'ú', 'ü', 'û'}}
                                 };

            return dictionary.Keys.Aggregate(text, (current1, key) => dictionary[key].Aggregate(
                current1, (current, symbol) => current.Replace(symbol, key)));
        }

        /// <summary>
        /// Convert <paramref name="text"/> to friendly url
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>Friendly url text</returns>
        public static string FriendlyUrl(this string text)
        {
            var builder = new StringBuilder();

            text = Clear((text ?? "").Trim().ToLower());

            foreach (var ch in text)
            {
                switch (ch)
                {
                    case ' ':
                        builder.Append('-');
                        break;
                    case '&':
                        builder.Append("and");
                        break;
                    case '\'':
                        break;
                    default:
                        if ((ch >= '0' && ch <= '9') ||
                            (ch >= 'a' && ch <= 'z'))
                        {
                            builder.Append(ch);
                        }
                        else
                        {
                            builder.Append('-');
                        }
                        break;
                }
            }

            return builder.ToString();
        }
    }
}
