using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace VeterinaryClinic.Helpers
{
    public class BlobHelper : IBlobHelper
    {
        private readonly CloudBlobClient _blobClient;
        public BlobHelper(IConfiguration configuration)
        {
            string keys = configuration["Blob:ConnectionString"];
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(keys);
            _blobClient = storageAccount.CreateCloudBlobClient();
        }

        /// <summary>
        /// / Método que faz o upload das imagens blob.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="containerName"></param>
        /// <returns>stream, containerName</returns>
        public async Task<Guid> UploadBlobAsync(IFormFile file, string containerName)
        {
            Stream stream = file.OpenReadStream();
            return await UploadStreamAsync(stream, containerName);
        }
        /// <summary>
        ///  Método que faz o upload das imagens blob.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="containerName"></param>
        /// <returns>stream, containerName</returns>
        public async Task<Guid> UploadBlobAsync(byte[] file, string containerName)
        {
            MemoryStream stream = new MemoryStream(file);
            return await UploadStreamAsync(stream, containerName);
        }
        /// <summary>
        ///  Método que faz o upload das imagens blob.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="containerName"></param>
        /// <returns></returns>
        public async Task<Guid> UploadBlobAsync(string image, string containerName)
        {
            Stream stream = File.OpenRead(image);
            return await UploadStreamAsync(stream, containerName);
        }
        /// <summary>
        ///  Método que faz o upload das imagens blob.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="containerName"></param>
        /// <returns></returns>

        private async Task<Guid> UploadStreamAsync(Stream stream, string containerName)
        {
            Guid name = Guid.NewGuid();
            CloudBlobContainer container = _blobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{name}");
            await blockBlob.UploadFromStreamAsync(stream);
            return name;
        }


    }
}
