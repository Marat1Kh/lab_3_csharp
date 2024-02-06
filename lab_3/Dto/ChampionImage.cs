using System.Text.Json.Serialization;

namespace lab_3.Models;

public class ChampionImage
{
    [JsonPropertyName("full")]
    public string Full { get; set; }
    
}