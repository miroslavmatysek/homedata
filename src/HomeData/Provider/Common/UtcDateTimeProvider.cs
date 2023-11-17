namespace HomeData.Provider.Common;

public class UtcDateTimeProvider : ITimeProvider
{
    public DateTime GetNow()
    {
        return DateTime.UtcNow;
    }
}