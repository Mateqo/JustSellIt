using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Domain.Model
{
    public class OwnerContact
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public int OwnerRef { get; set; }
        public Owner Owner { get; set; }
    }
}
