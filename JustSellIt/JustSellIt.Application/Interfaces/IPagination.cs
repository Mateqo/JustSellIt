using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Application.Interfaces
{
    public interface IPagination
    {
        int PageSize { get; set; }
        int? ActualPage { get; set; }
        bool IsNewSearch { get; set; }
        int Count { get; set; }
    }
}
