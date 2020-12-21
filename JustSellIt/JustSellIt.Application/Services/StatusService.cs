using JustSellIt.Application.Interfaces;
using JustSellIt.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustSellIt.Application.Services
{
    public class StatusService : IStatusService
    {
        private readonly IProductRepository _productRepo;

        public StatusService(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        public int GetIdBeforeNew()
        {
            var id=_productRepo.GetAllStatuses().FirstOrDefault(x=>x.Name== "BeforeNew").Id;
            return id;
        }
        public int GetIdDraft()
        {
            var id = _productRepo.GetAllStatuses().FirstOrDefault(x => x.Name == "Draft").Id;
            return id;
        }
        public int GetIdForVeryfication()
        {
            var id = _productRepo.GetAllStatuses().FirstOrDefault(x => x.Name == "ForVeryfication").Id;
            return id;
        }
        public int GetIdPublished()
        {
            var id = _productRepo.GetAllStatuses().FirstOrDefault(x => x.Name == "Published").Id;
            return id;
        }
        public int GetIdRejected()
        {
            var id = _productRepo.GetAllStatuses().FirstOrDefault(x => x.Name == "Rejected").Id;
            return id;
        }
        public int GetIdDeleted()
        {
            var id = _productRepo.GetAllStatuses().FirstOrDefault(x => x.Name == "Deleted").Id;
            return id;
        }
    }
}
