//-----------------------------------------------------------------------
// <copyright file="ImageUpload.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace RepositoryLayer.PostImage
{
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using System;

    public class ImageUpload
    {
        public readonly IConfiguration configuration;
        public IFormFile file;
        public ImageUpload(IConfiguration configuration, IFormFile file)
        {
            this.configuration = configuration;
            this.file = file;
        }
        public string Upload(IFormFile file)
        {
            try
            {
                var cloudeName = configuration["Cloudinary:CloudName"];
                var keyName = configuration["Cloudinary:APIKey"];
                var secretKey = configuration["Cloudinary:APISecret"];

                Account account = new Account()
                {
                    Cloud = cloudeName,
                    ApiKey = keyName,
                    ApiSecret = secretKey
                };

                Cloudinary cloudinary = new Cloudinary(account);
                var fileValue = file.FileName;
                var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(fileValue, stream)
                };

                var uploadResult = cloudinary.Upload(uploadParams);
                return uploadResult.Uri.ToString();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
