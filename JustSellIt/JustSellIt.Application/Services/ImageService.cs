using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using JustSellIt.Application.Interfaces;
using JustSellIt.Application.ViewModels.Product;
using JustSellIt.Domain.Interface;
using JustSellIt.Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace JustSellIt.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ImageService(IImageRepository imageRepo, IMapper mapper, IConfiguration configuration)
        {
            _imageRepo = imageRepo;
            _mapper = mapper;
            _configuration = configuration;
        }

        public void AddImages(List<ImageProductVm> images, int productId)
        {
            foreach (var image in images)
            {
                _imageRepo.AddImage(_mapper.Map<Image>(image));
            }
        }

        public void UpdateImages(List<ImageProductVm> images, int productId)
        {
            var imagesBefore = _imageRepo.GetImages(productId);
            foreach (var image in imagesBefore)
            {
                if (images.Any(x => x.Id == image.Id))
                {
                    var imageToUpdate = images.FirstOrDefault(x => x.Id == image.Id);
                    image.Name = imageToUpdate.Name;
                    image.isMain = imageToUpdate.isMain;

                    _imageRepo.Update(image);
                }
                else
                {
                    _imageRepo.DeleteImage(image.Id);
                }
            }
        }

        public bool DeleteImages(int productId)
        {
            return _imageRepo.DeleteImagesByProductId(productId);
        }

        public List<ImageProductVm> GetImages(int productId)
        {
            var images = _imageRepo.GetImages(productId);
            return images.ProjectTo<ImageProductVm>(_mapper.ConfigurationProvider).ToList();
        }

        public string UploadToAzure(IFormFile file)
        {
            string connectionString = _configuration.GetValue<string>("AzureConnection");
            BlobContainerClient container = new BlobContainerClient(connectionString, "product-images");
            container.CreateIfNotExists(PublicAccessType.Blob);

            var name = Guid.NewGuid().ToString();
            var blockBlob = container.GetBlobClient(name+".png");

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imageUrl);
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //Stream receiveStream = response.GetResponseStream();
            //receiveStream.Close();
            //response.Close();

            //if (file.Length > 0)
            //{
            //    using (var ms = new MemoryStream())
            //    {
            //        file.CopyTo(ms);
            //        // act on the Base64 data
            //        blockBlob.Upload(ms);
            //        blockBlob.Upload(ms);
            //    }
            //}
            blockBlob.Upload(file.OpenReadStream());

            return name;
        }

        public void DeleteToAzure(string imageName)
        {
            string connectionString = _configuration.GetValue<string>("AzureConnection");
            BlobContainerClient container = new BlobContainerClient(connectionString, "ProductImages");
            container.CreateIfNotExists(PublicAccessType.Blob);
            var blockBlob = container.GetBlobClient(imageName);

            blockBlob.DeleteIfExists();
        }
    }
}
