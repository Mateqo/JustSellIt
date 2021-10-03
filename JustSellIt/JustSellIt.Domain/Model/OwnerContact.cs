using System.ComponentModel.DataAnnotations;

namespace JustSellIt.Domain.Model
{
    public class OwnerContact
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public int OwnerRef { get; set; }
        public Owner Owner { get; set; }
    }
}
