namespace HomeData.Model.Config;

public class TaskConfig
{
    public string Name { get; set; }

    public string Measurement { get; set; }

    public string Bucket { get; set; }

    public Dictionary<string, string> Params { get; set; } = new Dictionary<string, string>();
}