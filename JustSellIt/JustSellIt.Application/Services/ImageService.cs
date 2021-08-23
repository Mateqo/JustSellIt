using AutoMapper;
using JustSellIt.Application.Interfaces;
using JustSellIt.Domain.Interface;

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
    }
}
