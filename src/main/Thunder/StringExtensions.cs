﻿using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Thunder
{
    /// <summary>
    /// Validator extensions
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
        /// <returns>Valid</returns>
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

            var calculado = d1 + d2.ToString();
            var digitado = n[9] + n[10].ToString();

            return calculado == digitado;
        }

        /// <summary>
        /// Check cnpj is valid
        /// </summary>
        /// <param name="cnpj">Cnpj</param>
        /// <returns>Valid</returns>
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
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            var resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            var digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;

            for (var i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

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
        /// <returns>Valid</returns>
        public static bool IsEmail(this string email)
        {
            return new Regex(@"^\w+([-+.]*[\w-]+)*@(\w+([-.]?\w+)){1,}\.\w{2,4}$").IsMatch(email);
        }

        /// <summary>
        /// Check date is valid
        /// </summary>
        /// <param name="date">date</param>
        /// <returns>Valid</returns>
        public static bool IsDate(this string date)
        {
            DateTime dt;
            return DateTime.TryParse(date, out dt);
        }
        /// <summary>
        /// Check hour is valid
        /// </summary>
        /// <param name="hour">Hour</param>
        /// <returns>Valid</returns>
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
        /// Url available
        /// </summary>
        /// <param name="httpUrl">Http Url</param>
        /// <returns></returns>
        public static bool UrlAvailable(this string httpUrl)
        {
            if (!httpUrl.StartsWith("http://") || !httpUrl.StartsWith("https://"))
                httpUrl = "http://" + httpUrl;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(httpUrl);
                
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";

                var response = (HttpWebResponse)request.GetResponse();
                return true;
            }
            catch
            {
                return false;
            }
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
            return s.Replace(" ", "");
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
    }
}
