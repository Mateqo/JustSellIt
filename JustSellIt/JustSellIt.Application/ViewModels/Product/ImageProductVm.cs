using JustSellIt.Application.Mapping;

namespace JustSellIt.Application.ViewModels.Product
{
    public class ImageProductVm : IMapFrom<JustSellIt.Domain.Model.Image>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsMain { get; set; }
        public int Position { get; set; }
        public int ProductId { get; set; }

        public ImageProductVm()
        {

        }

        public ImageProductVm(string name)
        {
            Name = name;
            Position = 0;
            IsMain = false;
        }

        public ImageProductVm(string name, int position)
        {
            Name = name;
            Position = position;
            IsMain = false;
        }

        public ImageProductVm(string name, int position, bool isMain)
        {
            Name = name;
            IsMain = isMain;
            Position = position;
        }

        public void Mapping(MappingProfile profile)
        {
            profile.CreateMap<JustSellIt.Domain.Model.Image, ImageProductVm>().ReverseMap();
        }
    }
}
