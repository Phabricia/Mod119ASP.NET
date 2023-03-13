using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CursoMOD119.Data;
using CursoMOD119.Models;
using CursoMOD119.lib;
using Microsoft.AspNetCore.Authorization;

namespace CursoMOD119.Controllers
{

    [Authorize(Policy = AppConstants.APP_POLICY)]
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index(string sort, string searchName, int? pageNumber)

        {
            if (_context.Client == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Items'  is null.");
            }
            ViewData["SearchName"] = searchName;


            IQueryable<Client> clientsSql = _context.Client;

            if (!string.IsNullOrEmpty(searchName))
            {
                clientsSql = clientsSql.Where(c => c.Name.Contains(searchName) || c.Email.Contains(searchName));
                
            }

            switch (sort)
            {
                case "name_desc":
                    clientsSql = clientsSql.OrderByDescending(x => x.Name);
                    break;
                case "name_asc":
                    clientsSql = clientsSql.OrderBy(x => x.Name);
                    break;
                case "birthday_desc":
                    clientsSql = clientsSql.OrderByDescending(x => x.Birthday);
                    break;
                case "birthday_asc":
                    clientsSql = clientsSql.OrderBy(x => x.Birthday);
                    break;
                case "email_asc":
                    clientsSql = clientsSql.OrderBy(x => x.Email);
                    break;
                case "email_desc":
                    clientsSql = clientsSql.OrderByDescending(x => x.Email);
                    break;
                case "active_asc":
                    clientsSql = clientsSql.OrderBy(x => x.Active);
                    break;
                case "active_desc":
                    clientsSql = clientsSql.OrderByDescending(x => x.Active);
                    break;
            }
            if (sort == "name_desc")
            {
                ViewData["NameSort"] = "name_asc";
            }
            else
            {
                ViewData["NameSort"] = "name_desc";
            }

            if (sort == "birthday_desc")
            {
                ViewData["BirthdaySort"] = "birthday_asc";
            }
            else
            {
                ViewData["BirthdaySort"] = "birthday_desc";
            }

            if (sort == "email_desc")
            {
                ViewData["EmailSort"] = "email_asc";
            }
            else
            {
                ViewData["EmailSort"] = "email_desc";
            }

            if (sort == "active_desc")
            {
                ViewData["ActiveSort"] = "active_asc";
            }
            else
            {
                ViewData["ActiveSort"] = "active_desc";
            }

            int pageSize = 5;

            var clients = await PaginatedList<Client>.CreateAsync(clientsSql, pageNumber ?? 1, pageSize);

            return View(clients);

        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Client == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Birthday,Email,Active")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Client == null)
            {
                return NotFound();
            }

            var client = await _context.Client.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Birthday,Email,Active")] Client client)
        {
            if (id != client.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.ID))
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
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Client == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Client == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Client'  is null.");
            }
            var client = await _context.Client.FindAsync(id);
            if (client != null)
            {
                _context.Client.Remove(client);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
          return (_context.Client?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
