namespace HomeData.Tasks.Solax.Extensions;

internal static class ConvertsExtensions
{
    private const int SignedDiff = 65536;
    internal static decimal? ToDecimal(this int[] values, int index, int precision, bool signed = false)
    {
        if (values == null || values.Length < index)
            return null;

        var item = signed ? values[index].ToSigned() : values[index];

        if (precision <= 0)
            return item;

        return decimal.Round(decimal.Divide(item, (decimal)Math.Pow(10, precision)), precision);
    }

    internal static int? ToInt(this int[] values, int index, bool signed = false)
    {
        if (values == null || values.Length < index)
            return null;

        return signed ? values[index].ToSigned() : values[index];

    }

    internal static int ToSigned(this int value)
    {
        if (value > short.MaxValue)
        {
            return value - SignedDiff;
        }

        return value;
    }
}