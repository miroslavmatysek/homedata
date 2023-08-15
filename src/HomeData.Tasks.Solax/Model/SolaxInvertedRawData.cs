using Newtonsoft.Json;

namespace HomeData.Tasks.Solax.Model;

public class SolaxInvertedRawData
{
    [JsonProperty(PropertyName = "ver")]
    public string Version { get; set; }
    
    [JsonProperty(PropertyName = "sn")]
    public string SerialNumber { get; set; }
    
    [JsonProperty(PropertyName = "type")]
    public int Type { get; set; }
    
    [JsonProperty(PropertyName = "Data")]
    public int[] Data { get; set; }
}