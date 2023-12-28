using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MyContactManagerData;
using MyContactsManagerServices;
using project.Models;
using projectModels;

namespace project.Controllers
{
    public class ContactsController : Controller

    {
        
        private readonly IContactsSerivce _conatactsSerivce;
        private readonly IStateService _statesSerivce;
        private static List<State> _allStates;
        private static SelectList _statesData;
        private IMemoryCache _cache;

        public ContactsController( IContactsSerivce contactsSerivce,IStateService stateService,IMemoryCache cache)
        {
            
            _cache = cache;
            _conatactsSerivce=contactsSerivce;
        _statesSerivce=stateService;    
            SetAllStatesCachingData();
              _statesData = new SelectList(_allStates, "Id", "Abbreviation");


   }
        public void SetAllStatesCachingData()
        {
            var allStates = new List<State>();
            if (!_cache.TryGetValue(ContactsCacheConstants.ALL_STATES, out allStates))
            {
                var allStatesData = Task.Run(()=> _statesSerivce.GetAllAsync()).Result;
                _cache.Set(ContactsCacheConstants.ALL_STATES, allStatesData, TimeSpan.FromDays(1));
                allStates = _cache.Get(ContactsCacheConstants.ALL_STATES) as List<State>;
            }
             _allStates = allStates;

        }
        private async Task UpdateStateAndResetModelState(Contacts contact)
        {
            ModelState.Clear();
            //var state = await _context.States.SingleOrDefaultAsync(x=>x.Id==contact.StateId);
            var state = _allStates.SingleOrDefault(x => x.Id == contact.StateId);
            contact.State = state;
            TryValidateModel(contact);

         }
        protected async Task<string> GetCurrentUserId() {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;

        
        }
        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            var userId = await GetCurrentUserId();
            var contacts = await _conatactsSerivce.GetAllAsync(userId);
            return View(contacts);
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = await GetCurrentUserId();
            var contacts = await _conatactsSerivce.GetAsync((int)id,userId);
               
            if (contacts == null)
            {
                return NotFound();
            }

            return View(contacts);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            ViewData["StateId"] = _statesData;
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,PhonePrimary,PhoneSecondary,BirthDate,StreetAddress1,StreetAddress2,City,StateId,Zip,UserId")] Contacts contacts)
        {
            UpdateStateAndResetModelState(contacts);
            if (ModelState.IsValid)
            {
                var userId = await GetCurrentUserId();
                contacts.UserId = userId;
                await _conatactsSerivce.AddOrUpdateAsync(contacts,userId);
                return RedirectToAction(nameof(Index));
            }
            ViewData["StateId"] = _statesData;
            return View(contacts);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = await GetCurrentUserId();
            
            var contacts =  await _conatactsSerivce.GetAsync((int)id, userId);
            if (contacts == null)
            {
                return NotFound();
            }
            ViewData["StateId"] = _statesData;
            return View(contacts);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,PhonePrimary,PhoneSecondary,BirthDate,StreetAddress1,StreetAddress2,City,StateId,Zip,UserId")] Contacts contacts)
        {
            if (id != contacts.Id)
            {
                return NotFound();
            }
           await UpdateStateAndResetModelState(contacts);
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = await GetCurrentUserId();
                    contacts.UserId = userId;
                    await _conatactsSerivce.AddOrUpdateAsync(contacts,userId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await ContactsExists(contacts.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StateId"] = _statesData;
            return View(contacts);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = await GetCurrentUserId();
            var contacts = await _conatactsSerivce.GetAsync((int)id, userId);
            if (contacts == null)
            {
                return NotFound();
            }

            return View(contacts);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = await GetCurrentUserId();

            await _conatactsSerivce.DeleteAsync(id,userId);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool>ContactsExists(int id)
        {
            var userId = await GetCurrentUserId();
            return await _conatactsSerivce.ExistsAsync(id, userId);
        }

    }
}
