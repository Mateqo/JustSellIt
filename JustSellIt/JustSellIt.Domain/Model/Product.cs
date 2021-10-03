using System;
using System.ComponentModel.DataAnnotations;

namespace JustSellIt.Domain.Model
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal? Price { get; set; }
        [Required]
        public bool IsNegotiate { get; set; }
        [Required]
        public bool IsNew { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string PhoneContact { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int OwnerId { get; set; }
        [Required]
        public int ProductStatusId { get; set; }
        [Required]
        public bool StorePolicy { get; set; }
        public string RejectionReason { get; set; }
        public string MainImageName { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        [Required]
        public string UserGuid { get; set; }

        public virtual Category Category { get; set; }
        public virtual ProductStatus ProductStatus { get; set; }
        public virtual Owner  Owner { get; set; }
    }
}
