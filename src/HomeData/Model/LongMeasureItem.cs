namespace HomeData.Model;

public class LongMeasureItem : MeasureItem
{
    public LongMeasureItem(string field)
        : base(field, MeasureItemValueType.Int64)
    {
    }

    public override bool IsGreatOrEquals(MeasureItem newItem)
    {
        if (ItemValue == null || newItem.ItemValue == null)
            return false;
        if (ItemValue.Equals(newItem.ItemValue))
            return true;

        try
        {
            return Convert.ToInt64(ItemValue) >= Convert.ToInt64(newItem.ItemValue);
        }
        catch
        {
            return false;
        }
    }

    public override object? Subtract(object? value)
    {
        if (ItemValue == null && value == null)
            return null;

        if (ItemValue == null)
            return -1.0m * Convert.ToInt64(value);

        if (value == null)
            return ItemValue;

        return Convert.ToInt64(ItemValue) - Convert.ToInt64(value);
    }

    public override MeasureItem CreateSimilar(string field)
    {
        return CopyMetaDataTo(new LongMeasureItem(field));
    }

    public override void SetZero()
    {
        ItemValue = 0L;
    }
}