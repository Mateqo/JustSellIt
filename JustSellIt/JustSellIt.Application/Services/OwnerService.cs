using JustSellIt.Application.Interfaces;
using JustSellIt.Domain.Interface;
using JustSellIt.Domain.Model;

namespace JustSellIt.Application.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepo;

        public OwnerService(IOwnerRepository ownerRepo)
        {
            _ownerRepo = ownerRepo;
        }

        public int AddOwner(Owner owner)
        {
            return _ownerRepo.AddOwner(owner);
        }
    }
}
