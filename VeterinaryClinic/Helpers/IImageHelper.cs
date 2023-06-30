using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string folder);


    }
}

