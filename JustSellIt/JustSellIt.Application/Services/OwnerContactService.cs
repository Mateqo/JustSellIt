using JustSellIt.Application.Interfaces;
using JustSellIt.Domain.Interface;
using JustSellIt.Domain.Model;

namespace JustSellIt.Application.Services
{
    public class OwnerContactService : IOwnerContactService
    {
        private readonly IOwnerContactRepository _ownerContactRepo;

        public OwnerContactService(IOwnerContactRepository ownerContactRepo)
        {
            _ownerContactRepo = ownerContactRepo;
        }

        public int AddOwnerContact(OwnerContact contact)
        {
            return _ownerContactRepo.AddOwnerContact(contact);
        }

    }
}
