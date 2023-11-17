namespace HomeData.Model;

public abstract class MeasureItem
{
    public string Field { get; }

    public DateTime DateTime { get; set; }

    public DateTime LastChanged { get; set; }

    public TimeSpan ChangedInterval
    {
        get => DateTime - LastChanged;
    }

    public bool Changed { get; set; }

    public object? ItemValue { get; set; }

    public MeasureItemType Type { get; set; }

    public MeasureItemValueType ValueType { get; }

    public bool AggregateType
    {
        get => Type == MeasureItemType.Aggregate;
    }

    public MeasureItem(string field, MeasureItemValueType valueType)
    {
        Field = field;
        Changed = false;
        Type = MeasureItemType.Value;
        ValueType = valueType;
    }

    private bool Equals(MeasureItem other)
    {
        return Field == other.Field;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((MeasureItem)obj);
    }

    public override int GetHashCode()
    {
        return Field.GetHashCode();
    }

    public abstract bool IsGreatOrEquals(MeasureItem newItem);

    public bool CompareValue(MeasureItem newItem)
    {
        var result = false;
        if (ItemValue == null && newItem.ItemValue == null)
            result = true;
        else if (ItemValue == null || newItem.ItemValue == null)
            result = false;
        else if (ItemValue.Equals(newItem.ItemValue))
            result = true;

        return result;
    }

    public abstract object? Subtract(object? value);

    public abstract MeasureItem CreateSimilar(string field);

    public abstract void SetZero();

    protected MeasureItem CopyMetaDataTo(MeasureItem item)
    {
        item.LastChanged = LastChanged;
        item.Type = Type;
        item.Changed = Changed;
        item.DateTime = DateTime;
        return item;
    }
}