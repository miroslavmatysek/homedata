namespace HomeData.Provider.Common;

public class LocalTimeProvider : ITimeProvider
{
    public DateTime GetNow()
    {
        return DateTime.Now;
    }
}