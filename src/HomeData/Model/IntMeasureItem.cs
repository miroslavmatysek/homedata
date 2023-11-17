namespace HomeData.Model;

public class IntMeasureItem : MeasureItem
{
    public IntMeasureItem(string field)
        : base(field, MeasureItemValueType.Int)
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
            return Convert.ToInt32(ItemValue) >= Convert.ToInt32(newItem.ItemValue);
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
            return -1.0 * Convert.ToInt32(value);

        if (value == null)
            return ItemValue;

        return Convert.ToInt32(ItemValue) - Convert.ToInt32(value);
    }

    public override MeasureItem CreateSimilar(string field)
    {
        return CopyMetaDataTo(new IntMeasureItem(field));
    }

    public override void SetZero()
    {
        ItemValue = 0;
    }
}