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
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int OwnerId { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public bool StorePolicy { get; set; }
        public string RejectionReason { get; set; }

        public virtual Category Category { get; set; }
        public virtual Owner  Owner { get; set; }
    }
}
