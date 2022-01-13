using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIProjectLibrary.DataAccess;
using WebAPIProjectLibrary.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebSongController : ControllerBase
    {
        SqlCrud crud; 
        public WebSongController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("Default");
            crud = new SqlCrud(connectionString);
        }        
        
        [HttpGet]
        public List<FullSongModel> GetAllSongs()
        {
            return crud.GetAllSongs();
        }

        [HttpGet("{id}")]
        public FullSongModel GetSongById(int id)
        {
            return crud.GetOneSongById(id);
        }

        [HttpPost]
        public void Post(FullSongModel fullSongModel)
        {
            crud.SaveFullSongModel(fullSongModel);
        }
      
    }
}
