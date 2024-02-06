using System.Collections.Generic;
using System.Text.Json.Serialization;
using Avalonia.Media.Imaging;

namespace lab_3.Models
{
    public class Champion
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        
        [JsonPropertyName("info")]
        public ChampionInfo Info { get; set; }
        
        public Bitmap ImageSource { get; set; }
        // [JsonPropertyName("attack")]
        // public int Attack { get; set; }
        //
        // [JsonPropertyName("defense")]
        // public int Defense { get; set; }
        // [JsonPropertyName("magic")]
        // public int Magic { get; set; }
        //
        // [JsonPropertyName("difficulty")]
        // public int Difficulty { get; set; }
        // public ChampionImage Image{ get; set; }
        // public string ImageUrl{get; set; }
    }
   
   
}