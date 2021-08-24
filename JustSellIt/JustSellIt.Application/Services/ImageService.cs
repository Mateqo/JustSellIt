using AutoMapper;
using AutoMapper.QueryableExtensions;
using JustSellIt.Application.Interfaces;
using JustSellIt.Application.ViewModels.Product;
using JustSellIt.Domain.Interface;
using JustSellIt.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace JustSellIt.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepo;
        private readonly IMapper _mapper;

        public ImageService(IImageRepository imageRepo, IMapper mapper)
        {
            _imageRepo = imageRepo;
            _mapper = mapper;
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
                if (images.Any(x=>x.Id == image.Id))
                {
                    var imageToUpdate = images.FirstOrDefault(x => x.Id == image.Id);
                    image.Url = imageToUpdate.Url;
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
    }
}
