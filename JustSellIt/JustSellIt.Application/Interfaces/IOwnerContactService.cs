using JustSellIt.Domain.Model;

namespace JustSellIt.Application.Interfaces
{
    public interface IOwnerContactService
    {
        int AddOwnerContact(OwnerContact contact);
    }
}
