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
        public Task<int> AddOrUpdateAsync(Contacts contacts)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Contacts contacts)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Contacts>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Contacts?> GetAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
