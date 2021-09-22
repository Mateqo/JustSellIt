using JustSellIt.Application.ViewModels.Product;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace JustSellIt.Application.Interfaces
{
    public interface IImageService
    {
        void AddImages(List<ImageProductVm> images);
        void UpdateImages(List<ImageProductVm> images, int productId);
        bool DeleteImages(int productId);
        List<ImageProductVm> GetImages(int productId);
        string UploadProductToAzure(IFormFile file);
        string UploadOwnerToAzure(IFormFile file);
        void DeleteFromAzure(string imageName);
        bool DeleteImage(int imageId);
        ImageProductVm GetImageByPosition(int productId, int position);
    }
}
