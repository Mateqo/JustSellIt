using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Application.Interfaces
{
    public interface ISearch
    {
        string SearchString { get; set; }
        string SearchLocation { get; set; }
        int? SearchCategory { get; set; }
    }
}
