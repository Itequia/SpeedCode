using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itequia.SpeedCode.Export.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this object str) => string.IsNullOrEmpty(str?.ToString());

        public static string ToStringSafe(this object str) => str.IsNullOrEmpty() ? "" : str.ToString();

        public static string CamelCase(this string variableName) =>
            string.Join('.', variableName.Split('.').Select(x => x[0].ToString().ToLower() + x[1..]));

        public static string DeCamelCase(this string variableName) =>
            string.Join('.', variableName.Split('.').Select(x => x[0].ToString().ToUpper() + x[1..]));

        public static byte[] ToByteArray(this string str) => Encoding.ASCII.GetBytes(str);

        public static bool ToBool(this object date) => bool.TryParse(date.ToStringSafe(), out var result) && result;
    }
}
