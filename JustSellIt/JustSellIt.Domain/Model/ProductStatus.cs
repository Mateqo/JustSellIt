using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JustSellIt.Domain.Model
{
    public class ProductStatus
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        //Our Status:
        //BeforeNew default status before create
        //Draft  
        //ForVeryfication
        //Published
        //Rejected
        //Deleted
    }

}
