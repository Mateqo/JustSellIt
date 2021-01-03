using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Domain.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string ColorHex { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
