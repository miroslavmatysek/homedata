namespace HomeData.Model.Config;

[Serializable]
public class TaskConfig
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; }

    public string Measurement { get; set; }

    public string Bucket { get; set; }

    public TimeSpan Interval { get; set; }

    public bool UtcTime { get; set; } = true;

    public Dictionary<string, string> Params { get; set; } = new();
}