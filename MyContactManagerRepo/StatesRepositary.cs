using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

using MyContactManagerData;
using projectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyContactManagerRepo
{
    public class StatesRepositary : IStateRepositary
    {
        private MyContactDBManagerContext _Context;
        public StatesRepositary(MyContactDBManagerContext context) { 
            _Context = context;
        }

        public async Task<int> AddOrUpdateAsync(State state)
        {
            if (state.Id > 0)
            {
                return await Update(state);

            }return await Insert(state);
        }
        public async Task<int> Insert(State state)
        {
            await _Context.States.AddAsync(state);
             await _Context.SaveChangesAsync();
            return state.Id;
        }
        public async Task<int> Update(State state)
        {
            var existingState = await _Context.States.FirstOrDefaultAsync(x=>x.Id ==state.Id);
            if (existingState is null) throw new Exception("State Not Found");
            existingState.Abbreviation= state.Abbreviation;
            existingState.Name = state.Name;
            await _Context.SaveChangesAsync();
            return state.Id;
        }
        public async Task<int> DeleteAsync(int id)
        {
            var existingState = await _Context.States.FirstOrDefaultAsync(x => x.Id == id);
            if (existingState is null) throw new Exception("State Not Found");
            await Task.Run(() => { _Context.States.Remove(existingState); });
            await _Context.SaveChangesAsync();
            return id;
        }

        public async Task<int> DeleteAsync(State state)
        {
            return await DeleteAsync(state.Id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
           return await _Context.States.AnyAsync(x=> x.Id == id);
        }

        public async Task<IList<State>> GetAllAsync()
        {
            return await _Context.States
                              .AsNoTracking()
                                 .ToListAsync();
        }

        public async Task<State?> GetAsync(int id)
        {
            return  await _Context.States 
                                  .AsNoTracking()
                                     .FirstOrDefaultAsync(x=> x.Id ==id);

        }

    }
}
