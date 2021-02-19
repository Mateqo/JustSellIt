using JustSellIt.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Application.Interfaces
{
    public interface IAdminService
    {
        ListProductForAdminListVm GetProductsToVerify(int? actualPage, int pageSize);
        void RejectProduct(int id,string reason);
        void AcceptProduct(int id);
    }
}
