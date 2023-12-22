using projectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyContactManagerRepo
{
    public class ContactsRepositary : IContactsRepositary
    {
        public async Task<int> AddOrUpdateAsync(Contacts contacts)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteAsync(Contacts contacts)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Contacts>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Contacts?> GetAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
