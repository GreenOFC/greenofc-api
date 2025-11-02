using _24hplusdotnetcore.Common.Constants;
using System;
using System.Globalization;
using System.Text;

namespace _24hplusdotnetcore.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        public static decimal? ToDecimal(this string input)
        {
            bool parseResult = decimal.TryParse(input, out decimal parsedValue);
            return parseResult ? parsedValue : default(decimal?);
        }
        public static int? ToNumber(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            string str = input.Replace(",", string.Empty);
            bool parseResult = int.TryParse(str, out int parsedValue);
            return parseResult ? parsedValue : default(int?);
        }
        public static string FormatDate(this string input, string type)
        {
            if (DateTime.TryParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            {
                return dateTime.ToString(type);
            }
            return string.Empty;
        }

        public static bool IsNumber(this string input)
        {
            bool flag = true;

            foreach (var item in input)
            {
                var isValidNumber = int.TryParse(item.ToString(), out int value);

                if (!isValidNumber)
                {
                    flag = false;
                }
            }

            return flag;
        }

        public static int CounWithTrim(this string input)
        {
            return input.Trim().Length;
        }

        public static DateTime? ToDate(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            char[] splilit = new char[3] { '/', '-', '.' };
            var formatedDate = input.Split(splilit);

            if (formatedDate.Length == 2)
            {
                input = formatedDate[0].PadLeft(2, '0') + "/" + formatedDate[1];
            }

            if (formatedDate.Length == 3)
            {
                input = formatedDate[0] + "/" + formatedDate[1].PadLeft(2, '0') + "/" + formatedDate[2];
            }

            var format = new string[] { "dd/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "d/MM/yyyy", "dd/MM/yy", "dd/M/yy", "d/M/yy", "d/MM/yy", "MM/yyyy" };

            var validDate = DateTime
                            .TryParseExact(input, format,
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out DateTime dt);

            return dt;
        }

        public static bool IsValidDateString(this string input)
        {
            char[] splilit = new char[3] { '/', '-', '.' };
            var formatedDate = input.Split(splilit);

            if (formatedDate.Length == 2)
            {
                input = formatedDate[0].PadLeft(2, '0') + "/" + formatedDate[1];
            }

            if (formatedDate.Length == 3)
            {
                input = formatedDate[0] + "/" + formatedDate[1].PadLeft(2, '0') + "/" + formatedDate[2];
            }

            var format = new string[] { "dd/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "d/MM/yyyy", "dd/MM/yy", "dd/M/yy", "d/M/yy", "d/MM/yy", "MM/yyyy" };

            var validDate = DateTime
                            .TryParseExact(input, format,
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out DateTime dt);
            return validDate;
        }

        public static string ConvertSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
                else
                {
                    sb.Append($"\\{c}");
                }
            }
            return sb.ToString();
        }
    }
}
