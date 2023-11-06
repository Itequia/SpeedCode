namespace Itequia.SpeedCode.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this object str) => string.IsNullOrEmpty(str?.ToString());

        public static string ToStringSafe(this object str) => str.IsNullOrEmpty() ? "" : str.ToString();
    }
}
