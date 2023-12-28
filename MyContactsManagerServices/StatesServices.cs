using MyContactManagerRepo;
using projectModels;

namespace MyContactsManagerServices
{
    public class StatesService : IStateService
    {
        private IStateRepositary _statesRepositary;

        public StatesService(IStateRepositary stateRepositary)
        { 
          _statesRepositary = stateRepositary;
        }

        public async Task<IList<State>> GetAllAsync()
        {
            var states =  await _statesRepositary.GetAllAsync();
            return states.OrderBy(x=>x.Name).ToList();
        }
         public async Task<State?> GetAsync(int id)
        {
            return await _statesRepositary.GetAsync(id);
        }
        public async Task<int> AddOrUpdateAsync(State state)
        {
            return await _statesRepositary.AddOrUpdateAsync(state);
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _statesRepositary.DeleteAsync(id);
        }

        public async Task<int> DeleteAsync(State state)
        {
           return await _statesRepositary.DeleteAsync(state);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _statesRepositary.ExistsAsync(id);
        }

      
    }
}
