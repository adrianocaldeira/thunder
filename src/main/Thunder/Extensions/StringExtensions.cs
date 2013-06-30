using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Thunder.Extensions
{
    /// <summary>
    /// String extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Remove last <paramref name="caracter"/> from <paramref name="string"/>.
        /// </summary>
        /// <param name="string">String</param>
        /// <param name="caracter">Caracter</param>
        /// <returns>String</returns>
        public static string RemoveLastCaracter(this string @string, string caracter)
        {
            return @string.LastIndexOf(caracter, StringComparison.Ordinal) == -1
                       ? @string
                       : @string.Substring(0, @string.LastIndexOf(caracter, StringComparison.Ordinal));
        }

        /// <summary>
        /// Define text if <paramref name="string"/> is null or empty
        /// </summary>
        /// <param name="string">String</param>
        /// <param name="text">Text</param>
        /// <returns>String</returns>
        public static string TextIfEmpty(this string @string, string text)
        {
            return string.IsNullOrEmpty(@string) ? text : @string;
        }

        /// <summary>
        /// Check cpf is valid
        /// </summary>
        /// <param name="cpf">Cpf</param>
        /// <returns>String is cpf</returns>
        public static bool IsCpf(this string cpf)
        {
            cpf = cpf.Replace(".", "").Replace("-", "");

            var reg = new Regex(@"(^(\d{3}.\d{3}.\d{3}-\d{2})|(\d{11})$)");

            if (!reg.IsMatch(cpf))
                return false;

            int d1;
            int d2;
            var soma = 0;

            var peso1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var peso2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            //Digitos
            var n = new int[11];

            // Se o tamanho for < 11 entao retorna como inválido
            if (cpf.Length != 11)
                return false;

            // Caso coloque todos os numeros iguais
            switch (cpf)
            {
                case "00000000000":
                    return false;
                case "11111111111":
                    return false;
                case "2222222222":
                    return false;
                case "33333333333":
                    return false;
                case "44444444444":
                    return false;
                case "55555555555":
                    return false;
                case "66666666666":
                    return false;
                case "77777777777":
                    return false;
                case "88888888888":
                    return false;
                case "99999999999":
                    return false;
            }

            try
            {
                n[0] = Convert.ToInt32(cpf.Substring(0, 1));
                n[1] = Convert.ToInt32(cpf.Substring(1, 1));
                n[2] = Convert.ToInt32(cpf.Substring(2, 1));
                n[3] = Convert.ToInt32(cpf.Substring(3, 1));
                n[4] = Convert.ToInt32(cpf.Substring(4, 1));
                n[5] = Convert.ToInt32(cpf.Substring(5, 1));
                n[6] = Convert.ToInt32(cpf.Substring(6, 1));
                n[7] = Convert.ToInt32(cpf.Substring(7, 1));
                n[8] = Convert.ToInt32(cpf.Substring(8, 1));
                n[9] = Convert.ToInt32(cpf.Substring(9, 1));
                n[10] = Convert.ToInt32(cpf.Substring(10, 1));
            }
            catch
            {
                return false;
            }

            for (int i = 0; i <= peso1.GetUpperBound(0); i++)
                soma += (peso1[i] * Convert.ToInt32(n[i]));

            int resto = soma % 11;

            if (resto == 1 || resto == 0)
                d1 = 0;
            else
                d1 = 11 - resto;

            soma = 0;

            for (var i = 0; i <= peso2.GetUpperBound(0); i++)
                soma += (peso2[i] * Convert.ToInt32(n[i]));

            resto = soma % 11;

            if (resto == 1 || resto == 0)
                d2 = 0;
            else
                d2 = 11 - resto;

            var calculado = d1 + d2.ToString(CultureInfo.InvariantCulture);
            var digitado = n[9] + n[10].ToString(CultureInfo.InvariantCulture);

            return calculado == digitado;
        }

        /// <summary>
        /// Check cnpj is valid
        /// </summary>
        /// <param name="cnpj">Cnpj</param>
        /// <returns>String is cnpj</returns>
        public static bool IsCnpj(this string cnpj)
        {
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            var reg = new Regex(@"(^(\d{2}.\d{3}.\d{3}/\d{4}-\d{2})|(\d{14})$)");

            if (!reg.IsMatch(cnpj))
                return false;

            if (cnpj.Length != 14)
                return false;

            switch (cnpj)
            {
                case "00000000000000":
                    return false;
                case "11111111111111":
                    return false;
                case "22222222222222":
                    return false;
                case "33333333333333":
                    return false;
                case "44444444444444":
                    return false;
                case "55555555555555":
                    return false;
                case "66666666666666":
                    return false;
                case "77777777777777":
                    return false;
                case "88888888888888":
                    return false;
                case "99999999999999":
                    return false;
            }

            var multiplicador1 = new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };


            var tempCnpj = cnpj.Substring(0, 12);
            var soma = 0;

            for (var i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString(CultureInfo.InvariantCulture)) * multiplicador1[i];

            var resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            var digito = resto.ToString(CultureInfo.InvariantCulture);
            tempCnpj = tempCnpj + digito;
            soma = 0;

            for (var i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString(CultureInfo.InvariantCulture)) * multiplicador2[i];

            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto;

            return cnpj.EndsWith(digito);
        }

        /// <summary>
        /// Check e-mail is valid
        /// </summary>
        /// <param name="email">E-mail</param>
        /// <returns>String is e-mail</returns>
        public static bool IsEmail(this string email)
        {
            return new Regex(@"^\w+([-+.]*[\w-]+)*@(\w+([-.]?\w+)){1,}\.\w{2,4}$").IsMatch(email);
        }

        /// <summary>
        /// Check date is valid
        /// </summary>
        /// <param name="date">date</param>
        /// <returns>String is date</returns>
        public static bool IsDate(this string date)
        {
            DateTime dt;
            return DateTime.TryParse(date, out dt);
        }
        /// <summary>
        /// Check hour is valid
        /// </summary>
        /// <param name="hour">Hour</param>
        /// <returns>String is hour</returns>
        public static bool IsHour(this string hour)
        {
            try
            {
                var dt = new DateTime(
                    DateTime.Today.Year,
                    DateTime.Today.Month,
                    DateTime.Today.Day,
                    Convert.ToInt32(hour.Split(':')[0]),
                    Convert.ToInt32(hour.Split(':')[1]),
                    0);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check url is valid
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="requireProtocol">Require protocol</param>
        /// <returns>Valid</returns>
        public static bool IsUrl(this string url, bool requireProtocol = true)
        {
            var regex = @"^(https?|ftp):\/\/(((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$";

            if (!requireProtocol)
            {
                regex = @"^((https?|ftp):\/\/)?(((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$";
            }

            return url != null && new Regex(regex, RegexOptions.Compiled | RegexOptions.IgnoreCase).Match(url).Length > 0;
        }

        /// <summary>
        /// Reduce
        /// </summary>
        /// <param name="s">String</param>
        /// <param name="count">Count</param>
        /// <param name="endings">Ending caracter</param>
        /// <returns>Reduce string</returns>
        public static string Reduce(this string s, int count, string endings)
        {
            if (count < endings.Length)
                throw new Exception("Failed to reduce to less then endings length.");
            
            var sLength = s.Length;
            var len = sLength;

            len += endings.Length;
            
            if (count > sLength)
                return s;
            
            s = s.Substring(0, sLength - len + count);
            s += endings;

            return s;
        }

        /// <summary>
        /// Remove spaces
        /// </summary>
        /// <param name="s">String</param>
        /// <returns>String</returns>
        public static string RemoveSpaces(this string s)
        {
            return s.Replace(" ", "").Trim();
        }

        /// <summary>
        /// Truncate string
        /// </summary>
        /// <param name="s">String</param>
        /// <param name="length">Length</param>
        /// <returns>String</returns>
        public static string Truncate(this string s, int length)
        {
            return s.Length < length ? s : s.Substring(0, length);
        }

        /// <summary>
        /// Check of string is null or empty
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// Format string with arguments
        /// </summary>
        /// <param name="format">Format</param>
        /// <param name="args">Argument</param>
        /// <returns></returns>
        public static string With(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        /// Join array with separator
        /// </summary>
        /// <param name="array">Array</param>
        /// <param name="separator">Separator</param>
        /// <returns></returns>
        public static string Join(this IEnumerable<object> array, string separator)
        {
            return array == null ? "" : string.Join(separator, array.ToArray());
        }

        /// <summary>
        /// Join array with separator
        /// </summary>
        /// <param name="array">Array</param>
        /// <param name="separator">Separator</param>
        /// <returns></returns>
        public static string Join(this object[] array, string separator)
        {
            return array == null ? "" : string.Join(separator, array);
        }

        /// <summary>
        /// Check if the string contains only digits or float-point
        /// </summary>
        /// <param name="s">String</param>
        /// <param name="floatpoint">Considered float-point</param>
        /// <returns>String contains only digits or float-point</returns>
        public static bool IsNumberOnly(this string s, bool floatpoint)
        {
            s = s.Trim();

            return s.Length != 0 && s.Where(c => !char.IsDigit(c)).All(c => floatpoint && (c == '.' || c == ','));
        }

        /// <summary>
        /// Check if the string can be parse as Double respective Int32
        /// </summary>
        /// <param name="s">String</param>
        /// <param name="floatpoint">Double is considered, otherwhise Int32 is considered</param>
        /// <returns>String contains only digits or float-point</returns>
        public static bool IsNumber(this string s, bool floatpoint)
        {
            int i;
            double d;
            var withoutWhiteSpace = s.RemoveSpaces();

            return floatpoint ? 
                double.TryParse(withoutWhiteSpace, NumberStyles.Any, Thread.CurrentThread.CurrentUICulture, out d) : 
                int.TryParse(withoutWhiteSpace, out i);
        }

        /// <summary>
        /// Replace \r\n or \n by <br /> in string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string NlToBr(this string s)
        {
            return s.Replace("\r\n", "<br />").Replace("\n", "<br />");
        }

        /// <summary>
        /// Remove accent
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string RemoveAccent(this string s)
        {
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(s);
            return Encoding.ASCII.GetString(bytes);
        }

        /// <summary>
        /// Transform string to SEO url
        /// </summary>
        /// <param name="s"></param>
        /// <param name="maxlenght">Maxlength</param>
        /// <returns></returns>
        public static string ToSeo(this string s, int maxlenght = 2000)
        {
            var str = s.RemoveAccent().ToLower();
            
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", " ").Trim();
            str = str.Substring(0, str.Length <= maxlenght ? str.Length : maxlenght).Trim();
            str = Regex.Replace(str, @"\s", "-");

            return str;             
        }

        /// <summary>
        /// Transform string to hash
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static long ToHash(this string s)
        {
            var provider = new MD5CryptoServiceProvider();

            return BitConverter.ToInt64(provider.ComputeHash(Encoding.Unicode.GetBytes(s)), 0);
        }
    }
}
