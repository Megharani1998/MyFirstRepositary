using projectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyContactsManagerServices
{
    public interface IContactsSerivce
    {

        Task<IList<Contacts>> GetAllAsync(string userId);
        Task<Contacts?> GetAsync(int id, string userId);
        Task<int> AddOrUpdateAsync(Contacts contacts, string userId);
        Task<int> DeleteAsync(int id, string userId);
        Task<int> DeleteAsync(Contacts contacts, string userId);
        Task<bool> ExistsAsync(int id, string userId);
    }
}
