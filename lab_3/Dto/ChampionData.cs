using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace lab_3.Models;

public class ChampionData
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("format")]
    public string Format { get; set; }

    [JsonPropertyName("version")]
    public string Version { get; set; }

    [JsonPropertyName("data")]
    public Dictionary<string, Champion> Data { get; set; }
   
}