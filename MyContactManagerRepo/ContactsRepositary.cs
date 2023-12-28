﻿using Microsoft.EntityFrameworkCore;
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
    public class ContactsRepositary : IContactsRepositary
    {
        private MyContactDBManagerContext _context;

        public ContactsRepositary(MyContactDBManagerContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<IList<Contacts>> GetAllAsync()
        {
            var results = await _context.Contacts.Include(x=>x.State).
               AsNoTracking().
              ToListAsync();
            
            return results.OrderBy(x=>x.LastName).ThenBy(x=> x.FirstName).ToList();
        }

        public async Task<Contacts?> GetAsync(int id)
        {
            var result = await _context.Contacts.AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            return result;
        }
        public async Task<int> AddOrUpdateAsync(Contacts contacts)
        {
            if (contacts.Id > 0)
            {
                return await Update(contacts);
            }
            return await Insert(contacts);


        }
        private async Task<int> Insert(Contacts contacts)
        {
            await GetExistingStateReferance(contacts);
            await _context.Contacts.AddAsync(contacts);
            await _context.SaveChangesAsync();
            return contacts.Id;
        }
        private async Task GetExistingStateReferance(Contacts contacts)
        {
            var existingState = await _context.States.SingleOrDefaultAsync(x => x.Id == contacts.StateId);
            if (existingState is not null)
            {
                contacts.State = existingState;
            }
        }
        private async Task<int> Update(Contacts contacts)
        {
            var existingContact = await _context.Contacts.SingleOrDefaultAsync(x => x.Id == contacts.Id);
            if (existingContact is null) throw new Exception("Contatc not found");

            existingContact.BirthDate = contacts.BirthDate;
            existingContact.FirstName = contacts.FirstName;
            existingContact.LastName = contacts.LastName;   
            existingContact.Email = contacts.Email;
              existingContact.City = contacts.City;
            existingContact.PhonePrimary = contacts.PhonePrimary;
            existingContact.PhoneSecondary = contacts.PhoneSecondary;
            existingContact.StreetAddress1 = contacts.StreetAddress1;
            existingContact.StreetAddress2 = contacts.StreetAddress2;
            existingContact.UserId = contacts.UserId;
            existingContact.StateId = contacts.StateId;
            existingContact.Zip = contacts.Zip;

            await _context.SaveChangesAsync();
            return contacts.Id;


        }

        public async Task<int> DeleteAsync(int id)
        {
            var existingContacts = await _context.Contacts.SingleOrDefaultAsync(x => x.Id == id);
            if (existingContacts is null) throw new Exception("could not delete contacts because unable to find contact");
            await Task.Run(() => { _context.Contacts.Remove(existingContacts); });
            await   _context.SaveChangesAsync();
            return id;
         }

        public async Task<int> DeleteAsync(Contacts contacts)
        {
            return await DeleteAsync(contacts.Id);
        }

        public async Task<bool> ExistsAsync(int id)
        {

            return await _context.Contacts.AsNoTracking().AnyAsync(x=>x.Id ==id);
        }

       
    }
}
