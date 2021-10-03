using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JustSellIt.Domain.Model
{
    public class Sex
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Owner> Owners { get; set; }
        //Our Sex:
        //Male=0,
        //Female=1
    }
}
