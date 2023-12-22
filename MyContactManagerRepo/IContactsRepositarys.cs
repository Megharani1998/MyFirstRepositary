using projectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyContactManagerRepo
{
    public interface IContactsRepositary
    {

        Task<IList<Contacts>> GetAllAsync();
        Task<Contacts?> GetAsync(int id);
        Task<int> AddOrUpdateAsync(Contacts contacts);
        Task<int> DeleteAsync(int id);
        Task<int> DeleteAsync(Contacts contacts);
        Task<bool> ExistsAsync(int id);
    }
}
