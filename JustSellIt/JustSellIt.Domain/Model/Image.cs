using System.ComponentModel.DataAnnotations;

namespace JustSellIt.Domain.Model
{
    public class Image
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsMain { get; set; }
        [Required]
        public int Position { get; set; }
        [Required]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
