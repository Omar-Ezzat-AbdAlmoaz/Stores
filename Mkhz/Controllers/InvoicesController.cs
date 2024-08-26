using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    public class InvoicesController : Controller
    {
        private readonly AppDbContext _context;
        
        public InvoicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Invoices
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var total = _context.invoices.Select(t => t.Total);
            ViewData["Total"] = total.Sum();

            var totalPaid = _context.invoices.Select(tp => tp.Paid);
            ViewData["TotalPaid"] = totalPaid.Sum();

            var totalResidual = _context.invoices.Select(tr => tr.Residual);
            ViewData["TotalResidual"] = totalResidual.Sum();

            return _context.invoices != null ?
                        View(await _context.invoices.ToListAsync()) :
                        Problem("Entity set 'AppDbContext.invoices'  is null.");
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormCollection req)
        {
            var total = _context.invoices.Select(t => t.Total);
            ViewData["Total"] = total.Sum();

            var totalPaid = _context.invoices.Select(tp => tp.Paid);
            ViewData["TotalPaid"] = totalPaid.Sum();

            var totalResidual = _context.invoices.Select(tr => tr.Residual);
            ViewData["TotalResidual"] = totalResidual.Sum();
            ViewBag.inv = req["inv"];
            string inv = Convert.ToString(ViewBag.inv);
            if (inv == null)
            {
                
                return View();
            }
            ViewData["Total"] = _context.invoices.Where(i => i.Client.Contains(inv)).Select(t => t.Total).Sum();
            ViewData["TotalPaid"] = _context.invoices.Where(i => i.Client.Contains(inv)).Select(t => t.Paid).Sum();

            return _context.invoices != null ?
                        View(await _context.invoices.Where(i=> i.Client.Contains(inv)).ToListAsync()) :
                        Problem("Entity set 'AppDbContext.products'  is null.");
        }
        // GET: Invoices/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.invoices
                .FirstOrDefaultAsync(m => m.Id == id);
            List<ProductWithQuantity> PWQ = _context.productWithQuantities.Where(p => p.InvoiceId == id).ToList();
            var Products = new List<(string, ProductWithQuantity)>();
            foreach (var p in PWQ)
            {
                Product pr = await _context.products.FindAsync(p.ProductId);
                Products.Add((pr.NameProduct, p));
            }

            ViewData["Products"] = Products;
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Client,DateTime,Total,Paid,Residual")] Invoice invoice)
        {
            invoice.DateTime = DateTime.Now;
            invoice.Total = 0;
            invoice.Residual = 0;
            invoice.Paid = 0;

            if (ModelState.IsValid)
            {
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details),invoice);
            }
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.invoices == null)
            {
                return NotFound();
            }

            List<ProductWithQuantity> PWQ = _context.productWithQuantities.Where(p => p.InvoiceId == id).ToList();
            var Products = new List<(string, ProductWithQuantity)>();
            foreach (var p in PWQ)
            {
                Product pr = await _context.products.FindAsync(p.ProductId);
                Products.Add((pr.NameProduct, p));
            }
            ViewData["Products"] = Products;

            var invoice = await _context.invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Client,DateTime,Total,Paid,Residual")] Invoice invoice)
        {

            if (id != invoice.Id)
            {
                return NotFound();
            }
            invoice.Residual = invoice.Total - invoice.Paid;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Id))
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
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.invoices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.invoices == null)
            {
                return Problem("Entity set 'AppDbContext.invoices'  is null.");
            }
            var invoice = await _context.invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.invoices.Remove(invoice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpPost, ActionName("DeleteAndReturn")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAndReturn(int id)
        {
            if (_context.invoices == null)
            {
                return Problem("Entity set 'AppDbContext.invoices'  is null.");
            }
            var invoice = await _context.invoices.FindAsync(id);

            var products = _context.productWithQuantities.Where(i => i.InvoiceId == id).ToList();
            foreach (var productItem in products)
            {
                var product = await _context.products.FindAsync(productItem.ProductId);
                if (product != null)
                {
                    product.ProductQuantity += productItem.Quantity;
                    product.sales -= productItem.Quantity;
                    _context.products.Update(product);
                }
            }
            if (invoice != null)
            {
                _context.invoices.Remove(invoice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Add(int? id)
        {
            if (id == null || _context.invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["prods"] = new SelectList(_context.products, "Id", "NameProduct");
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(IFormCollection req, int id)
        {
            if (id == null || _context.invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            ViewBag.num = req["num"];
            ViewBag.prod = req["prod"];
            ViewBag.price = req["price"];

            var prodc = await _context.products.FindAsync(Convert.ToInt32(ViewBag.prod));
            if (prodc.ProductQuantity >= Convert.ToInt32(ViewBag.num))
            {
                ProductWithQuantity p = new ProductWithQuantity();
                p.Quantity = Convert.ToInt32(ViewBag.num);
                p.ProductId = Convert.ToInt32(ViewBag.prod);
                p.InvoiceId = id;
                p.confirmed = true;
                p.Price = Convert.ToDecimal(ViewBag.price);
                p.Total = p.Quantity * p.Price;
                _context.productWithQuantities.Add(p);

                invoice.Total += p.Total;
                Product pr = await _context.products.FindAsync(p.ProductId);
                if(pr != null)
                {
                    pr.ProductQuantity -= p.Quantity;
                    pr.sales += p.Quantity;
                    _context.products.Update(pr);
                }
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details),invoice);
            }
            else
            {
                TempData["message"] = "لا يوجد كمية متوفرة";
                ViewData["prods"] = new SelectList(_context.products, "Id", "NameProduct");

                return View();
            }
        }

        //public async Task<IActionResult> Confirmed(int id)
        //{

        //    List<ProductWithQuantity> PWQ = _context.productWithQuantities.Where(p => p.InvoiceId == id).ToList();

        //    foreach (var p in PWQ)
        //    {
        //        Product pr = await _context.products.FindAsync(p.ProductId);
        //        if (p.confirmed)
        //        {
        //            pr.ProductQuantity -= p.Quantity;
        //            p.confirmed = false;
        //            _context.products.Update(pr);
        //        }
        //    }

        //    TempData["message"] = "تم تأكيد الفاتورة";

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
        [Authorize]
        public async Task<IActionResult> Clients (int id)
        {
            return _context.invoices != null ?
                        View(await _context.invoices.OrderBy(x => x.Client).ToListAsync()) :
                        Problem("Entity set 'AppDbContext.invoices'  is null.");
        }
        [HttpPost]
        public async Task<IActionResult> Clients(IFormCollection req)
        {
            ViewBag.client = req["client"];
            string client = Convert.ToString(ViewBag.client);
            if (client == null) return View();
            return _context.invoices != null ?
                        View(await _context.invoices.Where(c => c.Client.Contains(client)).OrderBy(x => x.Client).ToListAsync()) :
                        Problem("Entity set 'AppDbContext.invoices'  is null.");
        }

        [Authorize]
        public async Task<IActionResult> Residual(int id)
        {
            var residual = _context.invoices.Where(r => r.Residual > 0).OrderBy(x => x.Client).ToList();
            TempData["message"] = "العدد ";

            var totalResidual = _context.invoices.Select(tr => tr.Residual);
            ViewData["TotalResidual"] = totalResidual.Sum();

            if (residual != null)
                return View(residual);
            else
            {
                TempData["message"] = "لا يوجد ااجل";
                return View();
            }
        }

        private bool InvoiceExists(int id)
        {
            return (_context.invoices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
