namespace HomeData.Util;

public static class DirectoryExtensions
{
    public static int GetOrDefaultInt(this Dictionary<string, string>? data, string key, int defaultValue)
    {
        if (data == null || !data.ContainsKey(key))
            return defaultValue;
        return int.Parse(data[key]);
    }

    public static string GetOrDefaultString(this Dictionary<string, string>? data, string key, string defaultValue)
    {
        if (data == null || !data.ContainsKey(key))
            return defaultValue;
        return data[key];
    }
}