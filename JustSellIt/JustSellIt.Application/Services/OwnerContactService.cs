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

        public void DeactivateOwnerContact(int ownerId)
        {
            _ownerContactRepo.DeactivateOwnerContact(ownerId);
        }
        public OwnerContact GetOwnerContactByOwner(int ownerId)
        {
            return _ownerContactRepo.GetContactByOwner(ownerId);
        }


        public void UpdateOwnerContact(OwnerContact ownerContact)
        {
            _ownerContactRepo.UpdateOwnerContact(ownerContact);
        }

    }
}
