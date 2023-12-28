using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyContactManagerData;
using projectModels;

namespace project.Controllers
{
    [Authorize]
    public class ContactsController : Controller

    {

        private readonly MyContactDBManagerContext _context;
        private static List<State> _allStates;
        private static SelectList _statesData;

        public ContactsController(MyContactDBManagerContext context)
        {
             _context = context;
             _allStates = Task.Run(() => _context.States.ToListAsync()).Result;
              _statesData = new SelectList(_allStates, "Id", "Abbreviation");

        }
        private async Task UpdateStateAndResetModelState(Contacts contact)
        {
            ModelState.Clear();
            //var state = await _context.States.SingleOrDefaultAsync(x=>x.Id==contact.StateId);
            var state = _allStates.SingleOrDefault(x => x.Id == contact.StateId);
            contact.State = state;
            TryValidateModel(contact);

            
        }

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            var contacts = _context.Contacts.Include(c => c.State);
            return View(await contacts.ToListAsync());
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contacts = await _context.Contacts
                .Include(c => c.State)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contacts == null)
            {
                return NotFound();
            }

            return View(contacts);
        }

        [Authorize]

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
                _context.Contacts.AddAsync(contacts);
                await _context.SaveChangesAsync();
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

            var contacts = await _context.Contacts.FindAsync(id);
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
            UpdateStateAndResetModelState(contacts);
            if (ModelState.IsValid)
            {
                try
                {
                  _context.Contacts.Update(contacts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactsExists(contacts.Id))
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

            var contacts = await _context.Contacts
                .Include(c => c.State)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var contacts = await _context.Contacts.FindAsync(id);
            if (contacts != null)
            {
                _context.Contacts.Remove(contacts);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactsExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }
    }
}
