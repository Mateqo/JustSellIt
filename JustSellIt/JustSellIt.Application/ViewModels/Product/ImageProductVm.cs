using JustSellIt.Application.Mapping;

namespace JustSellIt.Application.ViewModels.Product
{
    public class ImageProductVm
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool isMain { get; set; }

        public void Mapping(MappingProfile profile)
        {
            profile.CreateMap<JustSellIt.Domain.Model.Image, ImageProductVm>();
        }
    }
}
