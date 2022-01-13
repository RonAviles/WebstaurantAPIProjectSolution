using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPIProjectLibrary.Models
{
    public class FullSongModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public byte[] FileData { get; set; }
        public ArtistModel Artist { get; set; }
        public AlbumModel Album { get; set; }
    }
}
