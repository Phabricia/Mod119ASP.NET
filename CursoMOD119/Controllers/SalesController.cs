using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CursoMOD119.Data;
using CursoMOD119.Models;
using CursoMOD119.Models.SalesViewModels;
using CursoMOD119.Models.ItemsViewModels;
using NToastNotify;
using Microsoft.AspNetCore.Mvc.Localization;
using CursoMOD119.lib;
using Microsoft.AspNetCore.Authorization;

namespace CursoMOD119.Controllers
{
    [Authorize(Policy = AppConstants.APP_POLICY)]
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly IHtmlLocalizer<Resource> _sharedLocalizer;

        public SalesController(ApplicationDbContext context, IToastNotification toastNotification,
            IHtmlLocalizer<Resource> sharedLocalizer)
        {
            _context = context;
            _toastNotification = toastNotification;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: Sales
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sales.Include(s => s.Client);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sales == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales
                .Include(s => s.Client)
                .Include(s => s.Items)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // GET: Sales/Create
        public IActionResult Create()
        {
            ViewData["ClientID"] = new SelectList(_context.Client, "ID", "Name");
            ViewData["ItemIDs"] = new MultiSelectList(_context.Items, "ID", "Name");

            var saleViewModel = new SaleViewModel();

            var items = _context.Items.ToList();

            //saleViewModel.SelectableItems = items.Select(item => new SelectableItemViewModel
            //    {
            //        ID = item.ID,
            //        Name = item.Name,
            //        Price = item.Price,
            //        Selected = false
            //    })
            //    .ToList();

            saleViewModel.SelectableItems = items.ConvertAll<SelectableItemViewModel>(i => i);

            return View(saleViewModel);
        }

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SaleDate,Amount,ClientID, ItemIDs, SelectableItems")] SaleViewModel saleViewModel)
        {
            if (ModelState.IsValid)
            {
                List<Item> items = new List<Item>();

                foreach (var selectableItem in saleViewModel.SelectableItems)
                {
                    if (selectableItem.Selected == true)
                    {
                        Item? item = _context.Items.Find(selectableItem.ID);

                        if (item != null)
                        {
                            items.Add(item);
                        }
                    }
                }
                Sale sale = saleViewModel;
                sale.Items = items;

                //Sale sale = new Sale { 
                //    SaleDate = saleViewModel.SaleDate,
                //    Amount = saleViewModel.Amount,
                //    ClientID = saleViewModel.ClientID,
                //    Items = items
                //};
                _context.Add(sale);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientID"] = new SelectList(_context.Client, "ID", "Name", saleViewModel.ClientID);
            return View(saleViewModel);
        }

        // GET: Sales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sales == null)
            {
                return NotFound();
            }

            var sale = _context.Sales
                .Include(sales => sales.Client)
                .Include(sales => sales.Items)
                .Single(sales => sales.ID == id);
            if (sale == null)
            {
                return NotFound();
            }


            var allBDItems = _context.Items.ToList();

            var selectableItems = allBDItems.Select(item => new SelectableItemViewModel
            {
                ID = item.ID,
                Name = item.Name,
                Price = item.Price,
                Selected = sale.Items.Contains(item)
            })
            .ToList();

            SaleViewModel saleViewModel = sale;
            saleViewModel.SelectableItems = selectableItems;

            //var saleViewModel = new SaleViewModel
            //{
            //    Amount = sale.Amount,
            //    ID = sale.ID,
            //    SaleDate = sale.SaleDate,
            //    ClientID = sale.ClientID,
            //    SelectableItems = selectableItems
            //};


            ViewData["ClientID"] = new SelectList(_context.Client, "ID", "Name", sale.ClientID);
            return View(saleViewModel);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,SaleDate,Amount,ClientID, SelectableItems")] SaleViewModel saleViewModel)    
        {
            if (id != saleViewModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var sale = await _context.Sales.Include(x => x.Items).FirstOrDefaultAsync(x => x.ID == saleViewModel.ID);
                    
                    if (sale == null)
                    {
                        return NotFound();
                    }
                    var items = new List<Item>();
                    foreach (var itemId in saleViewModel.SelectableItems)
                    {
                        if (itemId.Selected)
                        {
                            var item = _context.Items.SingleOrDefault(i => i.ID == itemId.ID);
                            if (item != null)
                            {
                                items.Add(item);
                            }
                        }
                    }
                    sale.Amount = saleViewModel.Amount;
                    sale.SaleDate = saleViewModel.SaleDate;
                    sale.ClientID = saleViewModel.ClientID;
                    sale.Items = items;

                    _context.Update(sale);
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage(
                        string.Format(_sharedLocalizer["Edited Sale successfully"].Value, sale.ID),
                        new ToastrOptions { Title = _sharedLocalizer["Sale Edit"].Value });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleExists(saleViewModel.ID))
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
            ViewData["ClientID"] = new SelectList(_context.Client, "ID", "Name", saleViewModel.ClientID);
            return View(saleViewModel);
        }

        // GET: Sales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sales == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales
                .Include(s => s.Client)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sales == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Sales'  is null.");
            }
            var sale = await _context.Sales.FindAsync(id);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaleExists(int id)
        {
          return (_context.Sales?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
