namespace Itequia.SpeedCode.Extensions
{
    public static class DoubleExtensions
    {
        public static double? ToDouble(this object number)
        {
            if (double.TryParse(number.ToStringSafe(), out var result)) return result;
            return null;
        }
    }
}
