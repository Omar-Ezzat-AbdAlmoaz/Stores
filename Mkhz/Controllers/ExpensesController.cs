using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mkhz.Data;
using Mkhz.Models;

namespace Mkhz.Controllers
{
    [Authorize]
    public class ExpensesController : Controller
    {
        private readonly AppDbContext _context;

        public ExpensesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Expenses
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var total = _context.Expens.Select(t => t.Total);
            ViewData["Total"] = total.Sum();

            return _context.Expens != null ? 
                          View(await _context.Expens.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Expens'  is null.");
        }

        // GET: Expenses/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Expens == null)
            {
                return NotFound();
            }

            var expens = await _context.Expens
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expens == null)
            {
                return NotFound();
            }

            return View(expens);
        }

        // GET: Expenses/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Expenses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NameExpens,DateTimeExpens,Total")] Expens expens)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expens);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expens);
        }

        // GET: Expenses/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Expens == null)
            {
                return NotFound();
            }

            var expens = await _context.Expens.FindAsync(id);
            if (expens == null)
            {
                return NotFound();
            }
            return View(expens);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameExpens,DateTimeExpens,Total")] Expens expens)
        {
            if (id != expens.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expens);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpensExists(expens.Id))
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
            return View(expens);
        }

        // GET: Expenses/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Expens == null)
            {
                return NotFound();
            }

            var expens = await _context.Expens
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expens == null)
            {
                return NotFound();
            }

            return View(expens);
        }

        // POST: Expenses/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Expens == null)
            {
                return Problem("Entity set 'AppDbContext.Expens'  is null.");
            }
            var expens = await _context.Expens.FindAsync(id);
            if (expens != null)
            {
                _context.Expens.Remove(expens);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpensExists(int id)
        {
          return (_context.Expens?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
