using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Domain.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public bool IsNegotiate { get; set; }
        public bool IsNew { get; set; }
        public string Location { get; set; }
        public string PhoneContact { get; set; }
        public int CategoryId { get; set; }
        public int OwnerId { get; set; }
        public int ProductStatusId { get; set; }
        public bool StorePolicy { get; set; }
        public string RejectionReason { get; set; }
        public string MainImageName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UserGuid { get; set; }

        public virtual Category Category { get; set; }
        public virtual ProductStatus ProductStatus { get; set; }
        public virtual Owner  Owner { get; set; }
    }
}
