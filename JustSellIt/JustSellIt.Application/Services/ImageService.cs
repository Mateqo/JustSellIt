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
            foreach (var image in images)
            {
                _imageRepo.AddImage(_mapper.Map<Image>(image));
            }
        }

        public void UpdateImages(List<ImageProductVm> images, int productId)
        {
            var imagesBefore = _imageRepo.GetImages(productId).ToList();
            foreach (var item in imagesBefore)
            {
                item.IsMain = false;
                _imageRepo.Update(item);
            }

            foreach (var image in images)
            {
                var imageToChange = _imageRepo.GetImageByPosition(productId, image.Position);
                if(imageToChange != null)
                {
                    imageToChange.Name = image.Name;
                    imageToChange.IsMain = image.IsMain;

                    _imageRepo.Update(imageToChange);
                }
                else
                {
                    _imageRepo.AddImage(_mapper.Map<Image>(image));
                }
            }

            var imageToMain = _imageRepo.GetImages(productId)
                .OrderBy(x=>x.Position)
                .FirstOrDefault();

            if(imageToMain != null)
            {
                imageToMain.IsMain = true;
                _imageRepo.Update(imageToMain);
            }
        }

        public bool DeleteImage(int imageId)
        {
            return _imageRepo.DeleteImage(imageId);
        }

        public bool DeleteImages(int productId)
        {
            return _imageRepo.DeleteImagesByProductId(productId);
        }

        public ImageProductVm GetImageByPosition(int productId, int position)
        {
            var image = _imageRepo.GetImageByPosition(productId, position);
            return _mapper.Map<ImageProductVm>(image);
        }

        public List<ImageProductVm> GetImages(int productId)
        {
            var images = _imageRepo.GetImages(productId);
            return images.ProjectTo<ImageProductVm>(_mapper.ConfigurationProvider).ToList();
        }

        public string UploadProductToAzure(IFormFile file)
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

        public string UploadOwnerToAzure(IFormFile file)
        {
            if (file == null)
                return null;

            string connectionString = _configuration.GetValue<string>("AzureConnection");
            BlobContainerClient container = new BlobContainerClient(connectionString, "owner-images");
            container.CreateIfNotExists(PublicAccessType.Blob);

            var name = Guid.NewGuid().ToString();
            var blockBlob = container.GetBlobClient(name + ".png");

            blockBlob.Upload(file.OpenReadStream());

            return name;
        }

        public void DeleteImageProductFromAzure(string imageName)
        {
            string connectionString = _configuration.GetValue<string>("AzureConnection");
            BlobContainerClient container = new BlobContainerClient(connectionString, "product-images");
            container.CreateIfNotExists(PublicAccessType.Blob);
            var blockBlob = container.GetBlobClient(imageName + ".png");

            blockBlob.DeleteIfExists();
        }

        public void DeleteImageOwnerFromAzure(string imageName)
        {
            string connectionString = _configuration.GetValue<string>("AzureConnection");
            BlobContainerClient container = new BlobContainerClient(connectionString, "owner-images");
            container.CreateIfNotExists(PublicAccessType.Blob);
            var blockBlob = container.GetBlobClient(imageName + ".png");

            blockBlob.DeleteIfExists();
        }
    }
}
