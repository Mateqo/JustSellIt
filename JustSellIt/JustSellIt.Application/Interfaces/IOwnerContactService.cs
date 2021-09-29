using JustSellIt.Domain.Model;

namespace JustSellIt.Application.Interfaces
{
    public interface IOwnerContactService
    {
        int AddOwnerContact(OwnerContact contact);
        void DeactivateOwnerContact(int ownerId);
        OwnerContact GetOwnerContactByOwner(int ownerId);
        void UpdateOwnerContact(OwnerContact ownerContact);
    }
}
