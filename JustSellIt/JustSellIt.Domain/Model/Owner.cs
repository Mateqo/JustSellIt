using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JustSellIt.Domain.Model
{
    public class Owner
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string AvatarImage { get; set; }
        [Required]
        public int SexId { get; set; }
        public string City { get; set; }
        [Required]
        public string UserGuid { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual Sex Sex { get; set; }
        public OwnerContact OwnerContact { get; set; }
    }
}
