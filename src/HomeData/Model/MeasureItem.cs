namespace HomeData.Model;

public class MeasureItem
{
    public string Field { get; }

    public DateTime DateTime { get; set; }

    public DateTime LastChanged { get; set; }

    public TimeSpan ChangedInterval
    {
        get => DateTime - LastChanged;
    }

    public bool Changed { get; set; }

    public object? Value { get; set; }

    public MeasureItem(string field)
    {
        Field = field;
        Changed = false;
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
}