using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Domain.Model
{
    public class Sex
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Owner> Owners { get; set; }
        //Our Sex:
        //Male=0,
        //Female=1
    }
}
