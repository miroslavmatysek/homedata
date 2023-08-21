using HomeData.DataAccess.Model;
using HomeData.Model;
using InfluxDB.Client.Writes;

namespace HomeData.DataAccess.Influxdb.Model;

public class InfluxMeasurementFieldContainer : IMeasurementFieldContainer
{
    private PointData _pointData;

    public InfluxMeasurementFieldContainer(PointData pd)
    {
        _pointData = pd;
    }

    public IMeasurementFieldContainer With(string name, string value)
    {
        _pointData = _pointData.Field(name, value);
        return this;
    }

    public IMeasurementFieldContainer With(string name, int? value)
    {
        if (value.HasValue)
            _pointData = _pointData.Field(name, value);
        return this;
    }

    public IMeasurementFieldContainer With(string name, decimal? value)
    {
        if (value.HasValue)
            _pointData = _pointData.Field(name, value);
        return this;
    }

    private IMeasurementFieldContainer With(string name, object? value)
    {
        if (value != null)
            _pointData = _pointData.Field(name, value);
        return this;
    }

    public IMeasurementFieldContainer With(MeasureItem? item)
    {
        if (item != null)
            With(item.Field, item.ItemValue);
        return this;
    }

    public PointData Data
    {
        get => _pointData;
    }
}