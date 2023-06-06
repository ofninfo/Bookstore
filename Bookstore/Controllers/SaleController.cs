using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bookstore.Data;
using Bookstore.Models;
using Bookstore.Commun;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Bookstore.Controllers
{
    [Authorize]
    public class SaleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SaleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sale
        public async Task<IActionResult> Index()
        {
            return _context.Sales != null ?
                    View(await _context.Sales
                        .Include(i => i.SaleItems)
                        .Include(i => i.Client)
                        .ToListAsync()) :
                Problem("Entity set 'ApplicationDbContext.Sales'  is null.");
        }

        // GET: Sale/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sales == null)
            {
                return NotFound();
            }

            var saleModel = await _context.Sales
                .Include(i => i.SaleItems)
                .Include(i => i.Client)
                .FirstOrDefaultAsync(m => m.SaleId == id);
            if (saleModel == null)
            {
                return NotFound();
            }

            return View(saleModel);
        }

        //// GET: Sale/Create
        //public IActionResult Create()
        //{
        //    var clients = _context.Clients.ToList();
        //    ViewBag.Clients = clients.Select(s => new SelectListItem() { Value = s.ClientId.ToString(), Text = s.Name }).ToList();

        //    return View();
        //}

        // GET: Sale/Create
        public async Task<IActionResult> Create()
        {
            var newSaleModel = new SaleModel()
            {
                Date = DateTime.Now,
                Status = SaleStatus.Open
            };

            _context.Add(newSaleModel);
            int saleId = await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Edit), new { id = newSaleModel.SaleId });
        }

        // GET: Sale/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sales == null)
            {
                var newSaleModel = new SaleModel()
                {
                    Date = DateTime.Now,
                    Status = SaleStatus.Open
                };

                _context.Add(newSaleModel);
                int saleId = await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Edit), new { id = newSaleModel.SaleId });
            }

            var saleModel = await _context.Sales
                .Include(i => i.SaleItems)
                .Include("SaleItems.Book")
                .FirstOrDefaultAsync(x => x.SaleId == id);
            if (saleModel == null)
            {
                return NotFound();
            }

            saleModel.Subtotal = saleModel.SaleItems.Sum(s => s.Subtotal);

            var clients = await _context.Clients.ToListAsync();
            ViewBag.Clients = clients.Select(s => new SelectListItem() { Value = s.ClientId.ToString(), Text = s.Name, Selected = ((saleModel.ClientId != null) ? s.ClientId == saleModel.ClientId : false) }).ToList();

            return View(saleModel);
        }

        // POST: Sale/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string action, [Bind("SaleId,Date,ClientId,Status,Subtotal,Discount,Total,SaleItems")] SaleModel saleModel)
        {
            if (id != saleModel.SaleId)
            {
                return NotFound();
            }

            //if (saleModel.ClientId == null)
            //{
            //    ModelState.AddModelError("ClientId", "The Client ID field is required.");
            //}

            switch (action)
            {
                case "Closed":
                    if (saleModel.Total == 0)
                    {
                        ModelState.AddModelError("SaleItems", "At least one sale item is required.");
                    }
                    break;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    switch (action)
                    {
                        case "Closed":
                            saleModel.Status = SaleStatus.Closed;
                            break;
                        case "Cancel":
                            saleModel.Status = SaleStatus.Canceled;
                            break;
                    }

                    //_context.UpdateRange(saleModel.SaleItems);
                    _context.Update(saleModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleModelExists(saleModel.SaleId))
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

            saleModel.Total = saleModel.SaleItems.Sum(s => s.Value);

            var clients = await _context.Clients.ToListAsync();
            ViewBag.Clients = clients.Select(s => new SelectListItem() { Value = s.ClientId.ToString(), Text = s.Name, Selected = ((saleModel.ClientId != null) ? s.ClientId == saleModel.ClientId : false) }).ToList();

            return View(saleModel);
        }

        // GET: Sale/Cancel/5
        public async Task<IActionResult> Cancel(int? id)
        {
            if (id == null || _context.Sales == null)
            {
                return NotFound();
            }

            var saleModel = await _context.Sales.FindAsync(id);
            if (saleModel == null)
            {
                return NotFound();
            }

            saleModel.Status = SaleStatus.Canceled;

            _context.Update(saleModel);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Sale/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sales == null)
            {
                return NotFound();
            }

            var saleModel = await _context.Sales
                   .Include(i => i.SaleItems)
                   .Include(i => i.Client)
                   .FirstOrDefaultAsync(m => m.SaleId == id);
            if (saleModel == null)
            {
                return NotFound();
            }

            return View(saleModel);
        }

        // POST: Sale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sales == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Sales'  is null.");
            }
            var saleModel = await _context.Sales.FindAsync(id);
            if (saleModel != null)
            {
                _context.Sales.Remove(saleModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaleModelExists(int id)
        {
            return (_context.Sales?.Any(e => e.SaleId == id)).GetValueOrDefault();
        }
    }
}
