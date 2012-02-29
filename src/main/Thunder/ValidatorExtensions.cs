using System;
using System.Text.RegularExpressions;

namespace Thunder
{
    /// <summary>
    /// Validator extensions
    /// </summary>
    public static class ValidatorExtensions
    {
        /// <summary>
        /// Check cpf is valid
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
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
        /// <param name="cnpj"></param>
        /// <returns></returns>
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
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmail(this string email)
        {
            //Cria expressão regular de cpf
            var reg = new Regex(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$");

            //Verifica se a expressão é válida
            return reg.IsMatch(email);
        }

        /// <summary>
        /// Check date is valid
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsDate(this string date)
        {
            DateTime dt;
            return DateTime.TryParse(date, out dt);
        }
        /// <summary>
        /// Check hour is valid
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
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
        /// Check year is bisixth
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool IsYearBisixth(int year)
        {
            return (year % 4 == 0 && (year % 400 == 0 || year % 100 != 0));
        }
    }
}
