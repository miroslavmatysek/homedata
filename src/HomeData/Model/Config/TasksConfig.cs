namespace HomeData.Model.Config;

[Serializable]
public class TasksConfig
{
    public List<TaskConfig> Items { get; set; } = new();
}