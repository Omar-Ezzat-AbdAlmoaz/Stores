using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mkhz.Data;
using Mkhz.Models;

namespace Mkhz.Controllers
{
    [Authorize]
    public class ProductWithQuantitiesController : Controller
    {
        private readonly AppDbContext _context;

        public ProductWithQuantitiesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ProductWithQuantities
        public async Task<IActionResult> Index()
        {
              return _context.productWithQuantities != null ? 
                          View(await _context.productWithQuantities.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.productWithQuantities'  is null.");
        }

        // GET: ProductWithQuantities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.productWithQuantities == null)
            {
                return NotFound();
            }

            var productWithQuantity = await _context.productWithQuantities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productWithQuantity == null)
            {
                return NotFound();
            }

            return View(productWithQuantity);
        }

        // GET: ProductWithQuantities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductWithQuantities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,Quantity,InvoiceId,confirmed,Price,Total")] ProductWithQuantity productWithQuantity)
        {

            if (ModelState.IsValid)
            {
                _context.Add(productWithQuantity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productWithQuantity);
        }

        // GET: ProductWithQuantities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.productWithQuantities == null)
            {
                return NotFound();
            }
            var productWithQuantity = await _context.productWithQuantities.FindAsync(id);

            if (productWithQuantity == null)
            {
                return NotFound();
            }

            var pr = await _context.products.FindAsync(productWithQuantity.ProductId);
            if (pr != null)
            {
                pr.ProductQuantity += productWithQuantity.Quantity;
                pr.sales -= productWithQuantity.Quantity;
                _context.products.Update(pr);
                
            }

            var Inv = await _context.invoices.FindAsync(productWithQuantity.InvoiceId);
            if (Inv != null)
            {
                Inv.Total -= productWithQuantity.Total;
                _context.invoices.Update(Inv);
                
            }

            await _context.SaveChangesAsync();
            return View(productWithQuantity);
        }

        // POST: ProductWithQuantities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,Quantity,InvoiceId,Confirmed,Price,Total")] ProductWithQuantity productWithQuantity)
        {
            if (id != productWithQuantity.Id)
            {
                return NotFound();
            }
            var pr = await _context.products.FindAsync(productWithQuantity.ProductId);
            var Inv = await _context.invoices.FindAsync(productWithQuantity.InvoiceId);

            if (ModelState.IsValid)
            {

                try
                {
                    if (pr != null)
                    {
                        if (pr.ProductQuantity < productWithQuantity.Quantity)
                        {
                            TempData["message"] = "لا يوجد كمية متوفرة";
                            productWithQuantity.Quantity = 0;
                            productWithQuantity.Total = 0;
                            _context.Update(productWithQuantity);
                            await _context.SaveChangesAsync();
                            return View(productWithQuantity);
                        }

                        pr.ProductQuantity -= productWithQuantity.Quantity;
                        pr.sales += productWithQuantity.Quantity;
                        _context.products.Update(pr);

                    }
                    productWithQuantity.Total = productWithQuantity.Price * productWithQuantity.Quantity;
                    if (Inv != null)
                    {
                        Inv.Total += productWithQuantity.Total;
                        _context.invoices.Update(Inv);
                    }

                    _context.Update(productWithQuantity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductWithQuantityExists(productWithQuantity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Edit),"Invoices",Inv);
            }
            return View(productWithQuantity);
        }

        // GET: ProductWithQuantities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.productWithQuantities == null)
            {
                return NotFound();
            }

            var productWithQuantity = await _context.productWithQuantities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productWithQuantity == null)
            {
                return NotFound();
            }

            var pr = await _context.products.FindAsync(productWithQuantity.ProductId);
            if (pr != null)
            {
                ViewData["product"] = pr.NameProduct;
            }
            var Inv = await _context.invoices.FindAsync(productWithQuantity.InvoiceId);
            if (Inv != null)
            {
                ViewBag.inv = Inv.Client;
            }

            

            return View(productWithQuantity);
        }

        // POST: ProductWithQuantities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.productWithQuantities == null)
            {
                return Problem("Entity set 'AppDbContext.productWithQuantities'  is null.");
            }
            var productWithQuantity = await _context.productWithQuantities.FindAsync(id);

            var pr = await _context.products.FindAsync(productWithQuantity.ProductId);
            var Inv = await _context.invoices.FindAsync(productWithQuantity.InvoiceId);

            if (productWithQuantity != null)
            {
                

                if (pr != null)
                {
                    pr.ProductQuantity += productWithQuantity.Quantity;
                    pr.sales -= productWithQuantity.Quantity;
                    _context.products.Update(pr);
                }
                
                if (Inv != null)
                {
                    Inv.Total -= productWithQuantity.Total;
                    _context.invoices.Update(Inv);
                }

                _context.productWithQuantities.Remove(productWithQuantity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Edit), "Invoices", Inv);
        }

        private bool ProductWithQuantityExists(int id)
        {
          return (_context.productWithQuantities?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
