using HomeData.Model;

namespace HomeData.DataAccess.Model;

public interface IMeasurementFieldContainer
{
    IMeasurementFieldContainer With(string name, string value);

    IMeasurementFieldContainer With(string name, int? value);

    IMeasurementFieldContainer With(string name, decimal? value);

    IMeasurementFieldContainer With(MeasureItem item);
}