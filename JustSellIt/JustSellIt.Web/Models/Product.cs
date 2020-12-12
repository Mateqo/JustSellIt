using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace JustSellIt.Web.Models
{
    public class Product
    {
        [DisplayName("Identyfikator")]
        public int Id { get; set; }
        [DisplayName("Nazwa")]
        public string Name { get; set; }
        [DisplayName("Kategoria")]
        public string Category { get; set; }
        [DisplayName("Właściciel")]
        public string Owner { get; set; }
    }
}
