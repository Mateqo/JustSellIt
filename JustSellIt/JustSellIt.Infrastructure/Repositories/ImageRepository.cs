using JustSellIt.Domain.Interface;
using JustSellIt.Domain.Model;
using System.Linq;

namespace JustSellIt.Infrastructure.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly Context _context;

        public ImageRepository(Context context)
        {
            _context = context;
        }

        public int AddImage(Image image)
        {
            _context.Images.Add(image);
            _context.SaveChanges();

            return image.Id;
        }

        public bool DeleteImage(int imageId)
        {
            var item = _context.Images.Find(imageId);
            if (item != null)
            {
                _context.Remove(item);
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public bool DeleteImagesByProductId(int productId)
        {
            var items = _context.Images.Where(x => x.ProductId == productId);
            if (items != null)
            {
                foreach (var item in items)
                {
                    _context.Remove(item);
                }

                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public IQueryable<Image> GetImages(int productId)
        {
            return _context.Images.Where(x => x.ProductId == productId);
        }

        public void Update(Image image)
        {
            _context.Attach(image);
            _context.Entry(image).Property("IsMain").IsModified = true;
            _context.SaveChanges();
        }
    }
}
