using HomeData.DataAccess.Model;
using HomeData.Model;

namespace HomeData.DataAccess.TimescaleDb.Model;

public class TimeScaleDbMeasurementFieldContainer : IMeasurementFieldContainer
{
    private readonly string _tableName;
    private readonly List<MeasureItem> _items = new();

    public TimeScaleDbMeasurementFieldContainer(string tableName)
    {
        _tableName = tableName;
    }


    public IMeasurementFieldContainer With(string name, string value)
    {
        return With(new StringMeasureItem(name)
        {
            ItemValue = value
        });
    }

    public IMeasurementFieldContainer With(string name, int? value)
    {
        return With(new IntMeasureItem(name)
        {
            ItemValue = value
        });
    }

    public IMeasurementFieldContainer With(string name, decimal? value)
    {
        return With(new DecimalMeasureItem(name)
        {
            ItemValue = value
        });
    }

    public IMeasurementFieldContainer With(string name, float? value)
    {
        return With(new FloatMeasureItem(name)
        {
            ItemValue = value
        });
    }

    public IMeasurementFieldContainer With(MeasureItem item)
    {
        _items.Add(item);
        return this;
    }
    
    public MeasureItem[] Data => _items.ToArray();

    public int Count
    {
        get => _items.Count;
    }
}