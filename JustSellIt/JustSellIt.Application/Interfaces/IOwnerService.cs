using JustSellIt.Domain.Model;

namespace JustSellIt.Application.Interfaces
{
    public interface IOwnerService
    {
        int AddOwner(Owner owner);
        void DeactivateOwner(string userGuid);
        Owner GetOwnerByGuid(string guid);
    }
}
