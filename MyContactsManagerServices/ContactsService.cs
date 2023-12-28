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
        public ContactsService(IContactsRepositary contactsRepositary ) {
                _contactsRepositary = contactsRepositary;
        
        }
        public async Task<IList<Contacts>> GetAllAsync()
        {
           return await _contactsRepositary.GetAllAsync();
        }

        public async Task<Contacts?> GetAsync(int id)
        {
            return await _contactsRepositary.GetAsync(id);
        }
        public async Task<int> AddOrUpdateAsync(Contacts contacts)
        {
            return await _contactsRepositary.AddOrUpdateAsync(contacts);
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _contactsRepositary.DeleteAsync(id);
        }

        public async Task<int> DeleteAsync(Contacts contacts)
        {
            return await _contactsRepositary.DeleteAsync(contacts);
        }

        public Task<bool> ExistsAsync(int id)
        {
            return _contactsRepositary.ExistsAsync(id);
        }

      
    }
}
