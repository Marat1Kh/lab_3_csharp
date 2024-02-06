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
      
    }
   
   
}