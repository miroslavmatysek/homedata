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

    public void Add(string field, object? value, bool changed = false, DateTime? lastChanged = null)
    {
        _data.Add(field, Create(field, value, changed, lastChanged));
    }

    public void AddOrUpdate(string field, object value, bool changed = false, DateTime? lastChanged = null)
    {
        _data[field] = Create(field, value, changed, lastChanged);
    }

    private MeasureItem Create(string field, object value, bool changed = false, DateTime? lastChanged = null)
    {
        return new MeasureItem(field)
        {
            DateTime = Time,
            LastChanged = lastChanged ?? Time,
            Changed = changed,
            ItemValue = value
        };
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
            if (_data.TryGetValue(item.Key, out var old))
            {
                var equals = Compare(old, item.Value);
                result.Add(item.Key, item.Value.ItemValue, !equals, equals ? old.LastChanged : null);
            }
            else
                result.Add(item.Key, item.Value.ItemValue, true);
        }

        return result;
    }

    private static bool Compare(MeasureItem oldItem, MeasureItem newItem)
    {
        var result = false;
        if (oldItem.ItemValue == null && newItem.ItemValue == null)
            result = true;
        else if (oldItem.ItemValue == null || newItem.ItemValue == null)
            result = false;
        else if (oldItem.ItemValue.Equals(newItem.ItemValue))
            result = true;

        if (result)
        {
            newItem.LastChanged = oldItem.LastChanged;
        }

        return result;

    }
}