using MyContactManagerRepo;
using projectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyContactsManagerServices
{
    public class ContactsService : IContactsSerivce
    {
        private IContactsRepositary _contactsRepositary;
        public ContactsService(IContactsRepositary contactsRepositary)
        {
            _contactsRepositary = contactsRepositary;

        }
        public async Task<IList<Contacts>> GetAllAsync(string userId)
        {
            return await _contactsRepositary.GetAllAsync(userId);
        }

        public async Task<Contacts?> GetAsync(int id, string userId)
        {
            return await _contactsRepositary.GetAsync(id, userId);
        }
        public async Task<int> AddOrUpdateAsync(Contacts contacts, string userId)
        {
            return await _contactsRepositary.AddOrUpdateAsync(contacts, userId);
        }

        public async Task<int> DeleteAsync(int id, string userId)
        {
            return await _contactsRepositary.DeleteAsync(id, userId);
        }

        public async Task<int> DeleteAsync(Contacts contacts, string userId)
        {
            return await _contactsRepositary.DeleteAsync(contacts, userId);
        }

        public Task<bool> ExistsAsync(int id, string userId)
        {
            return _contactsRepositary.ExistsAsync(id, userId);
        }

    }
}
