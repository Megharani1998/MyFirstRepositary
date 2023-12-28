using System;
using System.Collections.Generic;
using System.Linq;
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
    public class StatesController : Controller
    {
       
        //private readonly MyContactDBManagerContext _context;
        private readonly IStateService _stateService;
        private readonly IMemoryCache _cache;
        /*
        public StatesController(MyContactDBManagerContext context,IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        */
        public StatesController(IStateService statesService, IMemoryCache cache)
        {
            _stateService = statesService;
            _cache = cache;
        }

        // GET: States
        public async Task<IActionResult> Index()
        {
            var allStates = new List<State>();
            if (!_cache.TryGetValue(ContactsCacheConstants.ALL_STATES, out allStates))
            {
                var allStatesData = await _stateService.GetAllAsync()as List<State>;
                _cache.Set(ContactsCacheConstants.ALL_STATES, allStatesData, TimeSpan.FromDays(1));
                  return View(allStatesData);
            }
            return View(allStates);
        }

        // GET: States/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var state = await _context.States
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var state = await _stateService.GetAsync((int)id);
            if (state == null)
            {
                return NotFound();
            }

            return View(state);
        }

        // GET: States/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: States/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Abbreviation")] State state)
        {
            if (ModelState.IsValid)
            {
               // _context.Add(state);
                //await _context.SaveChangesAsync();

                await _stateService.AddOrUpdateAsync(state);
                _cache.Remove(ContactsCacheConstants.ALL_STATES);
                return RedirectToAction(nameof(Index));
            }
            return View(state);
        }

        // GET: States/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var state = await _stateService.GetAsync((int)id);
           // var state = await _context.States.FindAsync(id);
            if (state == null)
            {
                return NotFound();
            }
            return View(state);
        }

        // POST: States/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Abbreviation")] State state)
        {
            if (id != state.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // _context.Update(state);
                    //await _context.SaveChangesAsync();
                    await _stateService.AddOrUpdateAsync(state);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StateExists(state.Id))
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
            return View(state);
        }

        // GET: States/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var state = await _context.States
               // .FirstOrDefaultAsync(m => m.Id == id);
            var state = await _stateService.GetAsync((int)id);
            if (state == null)
            {
                return NotFound();
            }

            return View(state);
        }

        // POST: States/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            //var state = await _context.States.FindAsync(id);
            //_context.States.Remove(state);
            // await _context.SaveChangesAsync();
            await _stateService.DeleteAsync(id);
            _cache.Remove(ContactsCacheConstants.ALL_STATES);
            return RedirectToAction(nameof(Index));
        }

        private bool StateExists(int id)
        {
            return Task.Run(()=> _stateService.ExistsAsync(id)).Result;

        }
    }
}
