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

        public void AddImages(List<ImageProductVm> images)
        {
            int imagePosition = 1;

            foreach (var image in images)
            {
                image.Position = 1;
                _imageRepo.AddImage(_mapper.Map<Image>(image));
                imagePosition++;
            }
        }

        public void UpdateImages(List<ImageProductVm> images, int productId)
        {
            //var imagesBefore = _imageRepo.GetImages(productId);
            //foreach (var image in imagesBefore)
            //{
            //    if (images.Any(x => x.Name == image.Name))
            //    {
            //        var imageToUpdate = images.FirstOrDefault(x => x.Name == image.Name);
            //        image.Name = imageToUpdate.Name;
            //        image.isMain = imageToUpdate.IsMain;

            //        _imageRepo.Update(image);
            //    }
            //    else
            //    {
            //        _imageRepo.DeleteImage(image.Id);
            //    }
            //}
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
            if (file == null)
                return null;

            string connectionString = _configuration.GetValue<string>("AzureConnection");
            BlobContainerClient container = new BlobContainerClient(connectionString, "product-images");
            container.CreateIfNotExists(PublicAccessType.Blob);

            var name = Guid.NewGuid().ToString();
            var blockBlob = container.GetBlobClient(name + ".png");

            blockBlob.Upload(file.OpenReadStream());

            return name;
        }

        public void DeleteFromAzure(string imageName)
        {
            string connectionString = _configuration.GetValue<string>("AzureConnection");
            BlobContainerClient container = new BlobContainerClient(connectionString, "product-images");
            container.CreateIfNotExists(PublicAccessType.Blob);
            var blockBlob = container.GetBlobClient(imageName + ".png");

            blockBlob.DeleteIfExists();
        }
    }
}
