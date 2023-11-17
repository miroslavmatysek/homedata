namespace HomeData.Model;

public class StringMeasureItem : MeasureItem
{
    public StringMeasureItem(string field)
        : base(field, MeasureItemValueType.String)
    {
    }

    public override bool IsGreatOrEquals(MeasureItem newItem)
    {
        throw new NotSupportedException();
    }

    public override object? Subtract(object? value)
    {
        throw new NotSupportedException();
    }

    public override MeasureItem CreateSimilar(string field)
    {
        return CopyMetaDataTo(new StringMeasureItem(field));
    }

    public override void SetZero()
    {
        ItemValue = string.Empty;
    }
}