using JustSellIt.Domain.Model;
using System.Linq;

namespace JustSellIt.Domain.Interface
{
    public interface IImageRepository
    {
        int AddImage(Image image);
        bool DeleteImage(int imageId);
        bool DeleteImagesByProductId(int productId);
        IQueryable<Image> GetImages(int productId);
        void Update(Image image);

    }
}
