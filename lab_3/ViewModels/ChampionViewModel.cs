using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using lab_3.Models;
using ReactiveUI;

namespace lab_3.ViewModels
{
    public class ChampionViewModel : ViewModelBase
    {

        private readonly ChampionService _championService;

        public ObservableCollection<Champion> Champions { get; }



        public ChampionViewModel()
        {
            _championService = new ChampionService();
            Champions = new ObservableCollection<Champion>();
            LoadChampionsAsync();

        }

        private async void LoadChampionsAsync()
        {
            var champions = await _championService.GetChampionsAsync();
            if (champions != null)
            {
                foreach (var champion in champions)
                {
                    // Assuming GetImageUrlForChampion is a method that constructs the URL for a champion's image.
                    var imageUrl = await _championService.GetImageUrlForChampion(champion.Id);
                    var image = await LoadImageAsync(imageUrl);
                    champion.ImageSource =
                        image; // Assuming you have a property named ImageSource in your Champion model
                    Champions.Add(champion);
                }
            }
        }

        public async Task<Bitmap> LoadImageAsync(string imageUrl)
        {
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(imageUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        return new Bitmap(stream);
                    }
                }
            }

            return null;
        }


    }
}