using HomeData.DataAccess.Model;
using HomeData.Model;
using InfluxDB.Client.Writes;

namespace HomeData.DataAccess.Influxdb.Model;

public class InfluxMeasurementFieldContainer : IMeasurementFieldContainer
{
    private PointData _pointData;
    private int _count;

    public InfluxMeasurementFieldContainer(PointData pd)
    {
        _pointData = pd;
        _count = 0;
    }

    public IMeasurementFieldContainer With(string name, string value)
    {
        _pointData = _pointData.Field(name, value);
        _count++;
        return this;
    }

    public IMeasurementFieldContainer With(string name, int? value)
    {
        if (value.HasValue)
        {
            _count++;
            _pointData = _pointData.Field(name, value);
        }
        return this;
    }

    public IMeasurementFieldContainer With(string name, decimal? value)
    {
        if (value.HasValue)
        {
            _count++;
            _pointData = _pointData.Field(name, value);
        }

        return this;

    }

    public IMeasurementFieldContainer With(string name, float? value)
    {
        if (value.HasValue)
        {
            _count++;
            _pointData = _pointData.Field(name, value);
        }

        return this;
    }

    private IMeasurementFieldContainer With(string name, object? value)
    {
        if (value != null)
        {
            _count++;
            _pointData = _pointData.Field(name, value);
        }

        return this;
    }

    public IMeasurementFieldContainer With(MeasureItem? item)
    {
        if (item != null)
            With(item.Field, item.ItemValue);

        return this;
    }

    public int Count
    {
        get => _count;
    }


    public PointData Data
    {
        get => _pointData;
    }
}