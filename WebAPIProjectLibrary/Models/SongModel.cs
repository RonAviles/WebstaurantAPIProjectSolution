using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPIProjectLibrary.Models
{
    public class SongModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public byte[] FileData { get; set; }
    }
}
