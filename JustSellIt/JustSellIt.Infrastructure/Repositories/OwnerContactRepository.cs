using JustSellIt.Domain.Interface;
using JustSellIt.Domain.Model;
using System.Linq;

namespace JustSellIt.Infrastructure.Repositories
{
    public class OwnerContactRepository : IOwnerContactRepository
    {
        private readonly Context _context;

        public OwnerContactRepository(Context context)
        {
            _context = context;
        }

        public int AddOwnerContact(OwnerContact conctact)
        {
            _context.OwnersContact.Add(conctact);
            _context.SaveChanges();

            return conctact.Id;
        }

        public OwnerContact GetContactByOwner(int ownerId)
        {
            var contact = _context.OwnersContact.FirstOrDefault(x => x.OwnerRef == ownerId);

            return contact;
        }

        public void UpdateOwnerContact(OwnerContact contact)
        {
            _context.Attach(contact);
            _context.Entry(contact).Property("Email").IsModified = true;
            _context.Entry(contact).Property("PhoneNumber").IsModified = true;

            _context.SaveChanges();
        }

        public void DeactivateOwnerContact(int ownerId)
        {
            var ownerContact = _context.OwnersContact.FirstOrDefault(x => x.Id == ownerId);
            _context.OwnersContact.Remove(ownerContact);

            _context.SaveChanges();
        }
    }
}
