using JustSellIt.Domain.Model;

namespace JustSellIt.Domain.Interface
{
    public interface IOwnerRepository
    {
        int AddOwner(Owner owner);
        Owner GetOwnerByGuid(string guid);
        void UpdateOwner(Owner owner);
        void DeactivateOwner(string userGuid);
    }
}
