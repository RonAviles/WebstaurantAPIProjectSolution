using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using WebAPIProjectUI.Models;

namespace WebAPIProjectUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public List<SongModel> AllSongs;
        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGet()
        {
            AllSongs = await GetAllSongs();            
        }
        private async Task<List<SongModel>> GetAllSongs()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:44303/api/WebSong");
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                string responseText = await response.Content.ReadAsStringAsync();
                var allSongs = JsonSerializer.Deserialize<List<SongModel>>(responseText, options);
                return allSongs;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        //public void SeedInitialSong()
        //{
        //    FullSongModel song = new FullSongModel();

        //    var bytes = System.IO.File.ReadAllBytes(@"C:\Users\ronal\Documents\Code\WebAPIProjectSolution\WebAPIProjectLibrary\Assets\Music\Brooklin - Quincas Moreira.mp3");

        //    song.Title = "Brooklin";
        //    song.FileData = bytes;
        //    song.Artist = new ArtistModel { Name = "Quincas Moreira" };
        //    song.Album = new AlbumModel { Title = "Youtube Album" };

        //    crud.SaveFullSongModel(song);


        //   //remove reference to the Library after seeding to ensure you are properly working on this project. 

        //}
    }
}
