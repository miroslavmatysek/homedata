namespace HomeData.Model;

public class MeasureContainer
{
    private readonly Dictionary<string, MeasureItem> _data;
    public DateTime Time { get; }

    public MeasureContainer(DateTime time)
    {
        Time = time;
        _data = new Dictionary<string, MeasureItem>();
    }

    public void AddDecimal(string field, decimal? value, bool changed = false, DateTime? lastChanged = null,
        MeasureItemType type = MeasureItemType.Value)
    {
        Add(new DecimalMeasureItem(field)
        {
            DateTime = Time,
            LastChanged = lastChanged ?? Time,
            Changed = changed,
            ItemValue = value,
            Type = type
        });
    }

    public void AddInt(string field, int? value, bool changed = false, DateTime? lastChanged = null,
        MeasureItemType type = MeasureItemType.Value)
    {
        Add(new IntMeasureItem(field)
        {
            DateTime = Time,
            LastChanged = lastChanged ?? Time,
            Changed = changed,
            ItemValue = value,
            Type = type
        });
    }

    public void AddInt64(string field, long? value, bool changed = false, DateTime? lastChanged = null,
        MeasureItemType type = MeasureItemType.Value)
    {
        Add(new LongMeasureItem(field)
        {
            DateTime = Time,
            LastChanged = lastChanged ?? Time,
            Changed = changed,
            ItemValue = value,
            Type = type
        });
    }

    public void Add(MeasureItem item)
    {
        _data.Add(item.Field, item);
    }

    public void AddOrUpdate(MeasureItem item)
    {
        _data[item.Field] = item;
    }

    public void AddString(string field, string? value, bool changed = false, DateTime? lastChanged = null)
    {
        Add(new LongMeasureItem(field)
        {
            DateTime = Time,
            LastChanged = lastChanged ?? Time,
            Changed = changed,
            ItemValue = value,
            Type = MeasureItemType.Value
        });
    }

    public List<MeasureItem> Data
    {
        get => _data.Values.ToList();
    }

    public MeasureContainer Merge(MeasureContainer mc)
    {
        var result = new MeasureContainer(mc.Time);

        foreach (var item in mc._data)
        {
            var newItem = item.Value.CreateSimilar(item.Key);
            newItem.ItemValue = item.Value.ItemValue;
            newItem.Type = item.Value.Type;
            newItem.Changed = true;
            newItem.DateTime = item.Value.DateTime;

            if (_data.TryGetValue(item.Key, out var old))
            {
                newItem.Changed = !old.CompareValue(item.Value);
                if (!newItem.Changed)
                {
                    item.Value.LastChanged = old.LastChanged;
                    newItem.LastChanged = old.LastChanged;
                }
            }

            result.Add(newItem);
        }

        return result;
    }
}