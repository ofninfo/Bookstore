using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bookstore.Data;
using Bookstore.Models;
using Bookstore.ViewModels;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace Bookstore.Controllers
{
    [Authorize]
    public class SaleItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SaleItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SaleItem
        public async Task<IActionResult> Index()
        {
            return _context.SaleItems != null ?
                        View(await _context.SaleItems.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.SaleItems'  is null.");
        }

        // GET: SaleItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SaleItems == null)
            {
                return NotFound();
            }

            var saleItemModel = await _context.SaleItems
                .FirstOrDefaultAsync(m => m.SaleItemId == id);
            if (saleItemModel == null)
            {
                return NotFound();
            }

            return View(saleItemModel);
        }

        // GET: SaleItem/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // GET: SaleItem/Create
        //public async Task<IActionResult> Create()
        //{
        //    SaleItemViewModel saleItemViewModel = new SaleItemViewModel();

        //    saleItemViewModel.Books = _context.Books.Select(s => new SelectListItem() { Value = s.BookId.ToString(), Text = s.Title }).ToList();
        //    saleItemViewModel.Sales = _context.Sales.Select(s => new SelectListItem() { Value = s.SaleId.ToString(), Text = s.SaleId.ToString() }).ToList();


        //    return View(saleItemViewModel);
        //}

        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var books = await _context.Books.ToListAsync();

            ViewBag.Books = books.Select(s => new SelectListItem() { Value = s.BookId.ToString(), Text = s.Title }).ToList();

            SaleItemModel saleItemModel = new SaleItemModel()
            {
                SaleId = id.GetValueOrDefault(),
                Quantity = 1
            };

            return View(saleItemModel);
        }

        // POST: SaleItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SaleItemId,SaleId,Value,Quantity,Subtotal,BookId")] SaleItemModel saleItemModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(saleItemModel);
                await _context.SaveChangesAsync();

                #region Update Book Inventory

                var bookModel = await _context.Books.FindAsync(saleItemModel.BookId);
                if (bookModel != null)
                {
                    bookModel.Quantity = bookModel.Quantity - saleItemModel.Quantity;
                    _context.Update(bookModel);
                    await _context.SaveChangesAsync();
                }

                #endregion

                return RedirectToAction(nameof(Edit), "Sale", new { id = saleItemModel.SaleId });
            }

            var books = await _context.Books.ToListAsync();
            ViewBag.Books = books.Select(s => new SelectListItem() { Value = s.BookId.ToString(), Text = s.Title }).ToList();

            return View(saleItemModel);
        }

        // GET: SaleItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SaleItems == null)
            {
                return NotFound();
            }

            var saleItemModel = await _context.SaleItems.FindAsync(id);
            if (saleItemModel == null)
            {
                return NotFound();
            }

            var books = await _context.Books.ToListAsync();
            ViewBag.Books = books.Select(s => new SelectListItem() { Value = s.BookId.ToString(), Text = s.Title, Selected = (s.BookId == saleItemModel.BookId) }).ToList();

            return View(saleItemModel);
        }

        // POST: SaleItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SaleItemId,SaleId,Value,Quantity,Subtotal,BookId")] SaleItemModel saleItemModel)
        {
            if (id != saleItemModel.SaleItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(saleItemModel);
                    await _context.SaveChangesAsync();

                    #region Update Book Inventory

                    var bookModel = await _context.Books.FindAsync(saleItemModel.BookId);
                    if (bookModel != null)
                    {
                        bookModel.Quantity = bookModel.Quantity - saleItemModel.Quantity;
                        _context.Update(bookModel);
                        await _context.SaveChangesAsync();
                    }

                    #endregion
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleItemModelExists(saleItemModel.SaleItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Edit), "Sale", new { id = saleItemModel.SaleId });
            }

            var books = await _context.Books.ToListAsync();
            ViewBag.Books = books.Select(s => new SelectListItem() { Value = s.BookId.ToString(), Text = s.Title, Selected = (s.BookId == saleItemModel.BookId) }).ToList();

            return View(saleItemModel);
        }

        // GET: SaleItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SaleItems == null)
            {
                return NotFound();
            }

            var saleItemModel = await _context.SaleItems.FirstOrDefaultAsync(m => m.SaleItemId == id);
            if (saleItemModel == null)
            {
                return NotFound();
            }

            return View(saleItemModel);
        }

        // POST: SaleItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int? saleId = null;

            if (_context.SaleItems == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SaleItems'  is null.");
            }
            var saleItemModel = await _context.SaleItems.FindAsync(id);
            if (saleItemModel != null)
            {
                saleId = saleItemModel.SaleId;
                _context.SaleItems.Remove(saleItemModel);
                await _context.SaveChangesAsync();

                #region Update Book Inventory

                var bookModel = await _context.Books.FindAsync(saleItemModel.BookId);
                if (bookModel != null)
                {
                    bookModel.Quantity = bookModel.Quantity + saleItemModel.Quantity;
                    _context.Update(bookModel);
                    await _context.SaveChangesAsync();
                }

                #endregion

            }

            return RedirectToAction(nameof(Edit), "Sale", new { id = saleId });
        }

        private bool SaleItemModelExists(int id)
        {
            return (_context.SaleItems?.Any(e => e.SaleItemId == id)).GetValueOrDefault();
        }
    }
}
