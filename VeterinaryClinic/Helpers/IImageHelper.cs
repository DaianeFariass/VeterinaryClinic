using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace VeterinaryClinic.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string folder);


    }
}

