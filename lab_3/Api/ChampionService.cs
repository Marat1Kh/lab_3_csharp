using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Avalonia.Media.Imaging;
using lab_3.Models;
using Npgsql;

namespace lab_3.Models
{
    public class ChampionService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        // Fetches all champions data
        public async Task<IEnumerable<Champion>> GetChampionsAsync()
        {
            var latestVersion = await GetLatestVersionAsync();
            string url = $"https://ddragon.leagueoflegends.com/cdn/{latestVersion}/data/en_US/champion.json";
            string jsonResponse = await _httpClient.GetStringAsync(url);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var championData = JsonSerializer.Deserialize<ChampionData>(jsonResponse, options);
            var champions = new List<Champion>();
            if (championData != null && championData.Data != null)
            {
                foreach (var entry in championData.Data.Values)
                {
                    champions.Add(entry);

                }
            }

            return champions;
        }
        public async Task<string> GetLatestVersionAsync()
        {
            string url = "https://ddragon.leagueoflegends.com/api/versions.json";
            string jsonResponse = await _httpClient.GetStringAsync(url);
            var versions = JsonSerializer.Deserialize<List<string>>(jsonResponse);
            var latestVersion = versions?.FirstOrDefault() ?? string.Empty;
            if (!string.IsNullOrEmpty(latestVersion))
            {
                await SaveVersionToDatabaseAsync(latestVersion);
            }

            return latestVersion;
        }
        public async Task SaveVersionToDatabaseAsync(string versions)
        {
            var connectionString = "Host=trendz-dev-do-user-15469121-0.c.db.ondigitalocean.com;Port=25060;Database=app_db;Username=doadmin;Password=AVNS_q4icRH_f4m-CzUIeqKr;SslMode=Require;Trust Server Certificate=true"; // Use your actual connection string

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
        
                // Check if the version already exists to avoid duplicates
                var checkCmd = new NpgsqlCommand("SELECT COUNT(*) FROM versions WHERE versions = @versions", connection);
                checkCmd.Parameters.AddWithValue("versions", versions);
                var exists = (long)await checkCmd.ExecuteScalarAsync();
                if (exists == 0)
                {
                    using (var command = new NpgsqlCommand("INSERT INTO versions (versions) VALUES (@versions)", connection))
                    {
                        command.Parameters.AddWithValue("versions", versions);
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        public async Task<string> GetImageUrlForChampion(string championId)
        {
            // Fetch the latest version
            var latestVersion = await GetLatestVersionAsync();
            var imageUrl = $"https://ddragon.leagueoflegends.com/cdn/{latestVersion}/img/champion/{championId}.png";

            return imageUrl;
        }
    }
}