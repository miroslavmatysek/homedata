namespace HomeData.Model;

public class DateTimeMeasureItem : MeasureItem
{
    public DateTimeMeasureItem(string field)
        : base(field, MeasureItemValueType.DateTime)
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
            return Convert.ToDateTime(ItemValue) >= Convert.ToDateTime(newItem.ItemValue);
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
            return Convert.ToDateTime(value);

        if (value == null)
            return ItemValue;

        return Convert.ToDateTime(ItemValue) - Convert.ToDateTime(value);
    }

    public override MeasureItem CreateSimilar(string field)
    {
        return CopyMetaDataTo(new DateTimeMeasureItem(field));
    }

    public override void SetZero()
    {
        ItemValue = DateTime.MinValue;
    }
}