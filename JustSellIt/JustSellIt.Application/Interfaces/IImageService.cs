using JustSellIt.Application.ViewModels.Product;
using System.Collections.Generic;

namespace JustSellIt.Application.Interfaces
{
    public interface IImageService
    {
        void AddImages(List<ImageProductVm> images, int productId);
        void UpdateImages(List<ImageProductVm> images, int productId);
        bool DeleteImages(int productId);
        List<ImageProductVm> GetImages(int productId);
    }
}
