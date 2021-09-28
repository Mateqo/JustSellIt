using JustSellIt.Domain.Interface;
using JustSellIt.Domain.Model;
using System.Linq;

namespace JustSellIt.Infrastructure.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly Context _context;

        public OwnerRepository(Context context)
        {
            _context = context;
        }

        public int AddOwner(Owner owner)
        {
            _context.Owners.Add(owner);
            _context.SaveChanges();

            return owner.Id;
        }

        public Owner GetOwnerByGuid(string guid)
        {
            var owner = _context.Owners.FirstOrDefault(x => x.UserGuid == guid);

            return owner;
        }

        public void UpdateOwner(Owner owner)
        {
            _context.Attach(owner);
            _context.Entry(owner).Property("Name").IsModified = true;
            _context.Entry(owner).Property("AvatarImage").IsModified = true;
            _context.Entry(owner).Property("SexId").IsModified = true;
            _context.Entry(owner).Property("City").IsModified = true;

            _context.SaveChanges();
        }

        public void DeactivateOwner(string userGuid)
        {
            var owner = _context.Owners.FirstOrDefault(x => x.UserGuid == userGuid);
            _context.Owners.Remove(owner);

            _context.SaveChanges();
        }
    }
}
