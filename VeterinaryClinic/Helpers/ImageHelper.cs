using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace VeterinaryClinic.Helpers
{
    public class ImageHelper : IImageHelper
    {
        /// <summary>
        /// Método que faz o upload da imagem local.
        /// </summary>
        /// <param name="imageFile"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public async Task<string> UploadImageAsync(IFormFile imageFile, string folder)
        {
            string guid = Guid.NewGuid().ToString();
            string file = $"{guid}.jpg";

            string path = Path.Combine(
                     Directory.GetCurrentDirectory(),
                     $"wwwroot\\images\\{folder}",
                     file);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            return $"~/images/{folder}/{file}";
        }
    }
}
