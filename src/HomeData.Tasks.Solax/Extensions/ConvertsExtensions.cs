namespace HomeData.Tasks.Solax.Extensions;

public static class ConvertsExtensions
{
    private const int Signed16Diff = 65536;
    private const long Signed32Diff = 4294967296;

    public static decimal? ToDecimal(this int[] values, int index, int precision, bool signed = false)
    {
        if (values == null || values.Length < index)
            return null;

        var item = signed ? values[index].ToSigned16() : values[index];

        if (precision <= 0)
            return item;

        return item.ToDecimal(precision);
    }

    public static decimal? ToDecimal(this int value, int precision) => ((long)value).ToDecimal(precision);

    public static decimal? ToDecimal(this long value, int precision) =>
        decimal.Round(decimal.Divide(value, (decimal)Math.Pow(10, precision)), precision);

    public static int? ToInt(this int[] values, int index, bool signed = false)
    {
        if (values == null || values.Length < index)
            return null;

        return signed ? values[index].ToSigned16() : values[index];
    }

    public static long? ToAccumulatedInt(this int[] values, int index0, int index1)
    {
        var result = values.ToInt(index0) ?? 0;
        result += (values.ToInt(index1) ?? 0) * Signed16Diff;
        return result;
    }


    public static int ToSigned16(this int value)
    {
        if (value > short.MaxValue)
        {
            return value - Signed16Diff;
        }

        return value;
    }

    public static long ToSigned32(this long value)
    {
        if (value > int.MaxValue)
        {
            return value - Signed32Diff;
        }

        return value;
    }
}