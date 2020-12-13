using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Domain.Model
{
        public enum ProductStatus
        {
            BeforeNew=-1, //default status before create
            Draft=0,  
            ForVeryfication=10, 
            Published=20,
            Rejected=30,
            Deleted=40
        }
    
}
