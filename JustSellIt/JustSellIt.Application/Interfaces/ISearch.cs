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
        int? SearchMinPrice { get; set; }
        int? SearchMaxPrice { get; set; }
        string SearchCondition { get; set; }
        string Sorting { get; set; }
    }
}
