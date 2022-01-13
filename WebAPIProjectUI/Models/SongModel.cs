using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIProjectUI.Models
{

    public class SongModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public byte[] FileData { get; set; }
        public ArtistModel Artist { get; set; } = new ArtistModel();
        public AlbumModel Album { get; set; } = new AlbumModel();
    }



}
