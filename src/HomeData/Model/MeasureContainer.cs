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
        _data.Add(field, new MeasureItem(field)
        {
            DateTime = Time,
            LastChanged = lastChanged ?? Time,
            Changed = changed,
            Value = value
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
            if (_data.TryGetValue(item.Key, out var old))
            {
                var equals = Compare(old, item.Value);
                result.Add(item.Key, item.Value, !equals, equals ? old.LastChanged : null);
            }
            else
                result.Add(item.Key, item.Value, true);
        }

        return result;
    }

    private static bool Compare(MeasureItem oldItem, MeasureItem newItem)
    {
        var result = false;
        if (oldItem.Value == null && newItem.Value == null)
            result = true;
        else if (oldItem.Value == null || newItem.Value == null)
            result = false;
        else if (oldItem.Value.Equals(newItem.Value))
            result = true;

        if (result)
        {
            newItem.LastChanged = oldItem.LastChanged;
        }

        return result;

    }
}