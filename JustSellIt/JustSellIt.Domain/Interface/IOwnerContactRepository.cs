using JustSellIt.Domain.Model;

namespace JustSellIt.Domain.Interface
{
    public interface IOwnerContactRepository
    {
        int AddOwnerContact(OwnerContact conctact);
        OwnerContact GetContactByOwner(int ownerId);
        void UpdateOwnerContact(OwnerContact contact);
    }
}
