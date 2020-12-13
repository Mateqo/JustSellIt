using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Domain.Model
{
    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AvatarImage { get; set; }
        public Sex Sex { get; set; }
        public string City { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public OwnerContact OwnerContact { get; set; }
    }
}
