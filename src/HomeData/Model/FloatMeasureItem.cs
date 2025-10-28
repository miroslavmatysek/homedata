namespace HomeData.Model;

public class FloatMeasureItem : MeasureItem
{
    public FloatMeasureItem(string field)
        : base(field, MeasureItemValueType.Float)
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
            return Convert.ToSingle(ItemValue) >= Convert.ToSingle(newItem.ItemValue);
        }
        catch
        {
            return false;
        }
    }

    public override object Subtract(object value)
    {
        if (ItemValue == null && value == null)
            return null;

        if (ItemValue == null)
            return -1.0f * Convert.ToSingle(value);

        if (value == null)
            return ItemValue;

        return Convert.ToSingle(ItemValue) - Convert.ToSingle(value);
    }

    public override MeasureItem CreateSimilar(string field)
    {
        return CopyMetaDataTo(new FloatMeasureItem(field));
    }

    public override void SetZero()
    {
        ItemValue = 0.0f;
    }
}