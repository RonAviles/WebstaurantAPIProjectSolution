using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAPIProjectUI.Models;
using WebAPIProjectUI.Helpers;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace WebAPIProjectUI.Pages
{
    public class UploadModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UploadModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public string ArtistName { get; set; }
        [BindProperty]
        public string AlbumTitle { get; set; }
        [BindProperty]
        public IFormFile SongFile { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            SongModel songToUpload = new SongModel();
            songToUpload.Title = Title;
            songToUpload.Artist.Name = ArtistName;
            songToUpload.Album.Title = AlbumTitle;
            songToUpload.FileData = await SongFile.EncodeSongAsync();

            var client = _httpClientFactory.CreateClient();
            var jSonString = JsonSerializer.Serialize(songToUpload);
            var content = new StringContent(jSonString, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:44303/api/WebSong", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }

            return RedirectToPage("/Index");
        }
    }
}
