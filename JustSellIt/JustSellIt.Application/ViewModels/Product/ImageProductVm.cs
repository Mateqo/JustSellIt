using JustSellIt.Application.Mapping;

namespace JustSellIt.Application.ViewModels.Product
{
    public class ImageProductVm : IMapFrom<JustSellIt.Domain.Model.Image>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsMain { get; set; }
        public int ProductId { get; set; }

        public ImageProductVm()
        {

        }

        public ImageProductVm(string name)
        {
            Name = name;
            IsMain = false;
        }

        public ImageProductVm(string name, bool isMain)
        {
            Name = name;
            IsMain = isMain;
        }

        public void Mapping(MappingProfile profile)
        {
            profile.CreateMap<JustSellIt.Domain.Model.Image, ImageProductVm>().ReverseMap();
        }
    }
}
