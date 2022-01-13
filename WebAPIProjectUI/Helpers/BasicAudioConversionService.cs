using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIProjectUI.Helpers
{
    public static class BasicAudioConversionService
    {
        public static string DecodeSong(this byte[] data)
        {
            return $"data:audio/mp3;base64,{Convert.ToBase64String(data)}";
        }

        public static async Task<byte[]> EncodeSongAsync(this IFormFile file)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            return ms.ToArray();
        }

    }
}
