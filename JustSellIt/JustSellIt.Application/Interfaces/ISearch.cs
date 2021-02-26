using System;
using System.Collections.Generic;
using System.Text;
using JustSellIt.Application.ViewModels.Product;

namespace JustSellIt.Application.Interfaces
{
    public interface ISearch
    {
        string SearchString { get; set; }
        string SearchLocation { get; set; }
    }
}
