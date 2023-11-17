namespace HomeData.Model;

public class DecimalMeasureItem : MeasureItem
{
    public DecimalMeasureItem(string field) : base(field, MeasureItemValueType.Decimal)
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
            return Convert.ToDecimal(ItemValue) >= Convert.ToDecimal(newItem.ItemValue);
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
            return -1.0m * Convert.ToDecimal(value);

        if (value == null)
            return ItemValue;

        return Convert.ToDecimal(ItemValue) - Convert.ToDecimal(value);
    }

    public override MeasureItem CreateSimilar(string field)
    {
        return CopyMetaDataTo(new DecimalMeasureItem(field));
    }

    public override void SetZero()
    {
        ItemValue = 0.0m;
    }
}