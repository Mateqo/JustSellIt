using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JustSellIt.Domain.Model
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string ColorHex { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
