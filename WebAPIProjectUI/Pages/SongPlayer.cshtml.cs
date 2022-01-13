using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAPIProjectUI.Models;

namespace WebAPIProjectUI.Pages
{
    public class SongPlayerModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public SongPlayerModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public SongModel Song { get; set; }
        public async Task OnGet(int id)
        {
            Song = await GetSongById(id);
        }
        private async Task<SongModel> GetSongById(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:44303/api/WebSong/{id}");
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                string responseText = await response.Content.ReadAsStringAsync();
                var chosenSong = JsonSerializer.Deserialize<SongModel>(responseText, options);
                return chosenSong;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
