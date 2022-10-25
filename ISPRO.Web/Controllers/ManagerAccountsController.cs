using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ISPRO.Persistence.Context;
using ISPRO.Persistence.Entities;
using ISPRO.Helpers.Exceptions;
using ISPRO.Helpers;
using System.Text.RegularExpressions;
using ISPRO.Web.Authorization;

namespace ISPRO.Web.Controllers
{
    [AuthorizeUserLevel(UserLevelAuth.ADMIN)]
    public class ManagerAccountsController : Controller
    {
        private readonly DataContext _context;

        public ManagerAccountsController(DataContext context)
        {
            _context = context;
        }

        // GET: ManagerAccounts
        public async Task<IActionResult> Index()
        {
              return _context.ManagerAccounts != null ? 
                          View(await _context.ManagerAccounts.ToListAsync()) :
                          Problem("Entity set 'DataContext.ManagerAccounts'  is null.");
        }

        // GET: ManagerAccounts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.ManagerAccounts == null)
            {
                return NotFound();
            }

            var managerAccount = await _context.ManagerAccounts
                .FirstOrDefaultAsync(m => m.Username == id);
            if (managerAccount == null)
            {
                return NotFound();
            }

            return View(managerAccount);
        }

        // GET: ManagerAccounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ManagerAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,UserType,DisplayName,Password,Mobile,Email,ExpiryDate,Active,MaxAllowedProjects")] ManagerAccount managerAccount)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_context.ManagerAccounts.Any(u => u.Username.ToLower() == managerAccount.Username.Trim().ToLower() || u.Username.ToLower() == (managerAccount.Username.Trim().ToLower() + "@managers.com")))
                    {
                        ModelState.AddModelError("Username", "Username already taken.");
                    }
                    else
                    {
                        Regex rgx = new Regex("^[a-zA-Z0-9_]*$");
                        if (!rgx.IsMatch(managerAccount.Username.Trim()))
                        {
                            ModelState.AddModelError("Username", "Username could only be alphanumeric with '_'.");
                        }
                        else
                        {
                            managerAccount.Username = managerAccount.Username.Trim();
                            _context.Add(managerAccount);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }
            catch (ModelException ex)
            {
                ModelState.AddModelError("ModelError", ex.Message);
            }
            return View(managerAccount);
        }

        // GET: ManagerAccounts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.ManagerAccounts == null)
            {
                return NotFound();
            }

            var managerAccount = await _context.ManagerAccounts.FindAsync(id);
            if (managerAccount == null)
            {
                return NotFound();
            }
            return View(managerAccount);
        }

        // POST: ManagerAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Username,UserType,DisplayName,Password,Mobile,Email,ExpiryDate,Active,MaxAllowedProjects")] ManagerAccount managerAccount)
        {
            if (id != managerAccount.Username)
            {
                return NotFound();
            }

            new ReflectionHelper().CopyNullFromOld(await _context.ManagerAccounts.FindAsync(id), managerAccount);
            ModelState.Clear();
            TryValidateModel(managerAccount);

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Entry(_context.ManagerAccounts.Where(x => x.Username.Equals(id)).FirstOrDefault()).State = EntityState.Detached;
                    _context.Update(managerAccount);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManagerAccountExists(managerAccount.Username))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (ModelException ex)
                {
                    ModelState.AddModelError("ModelError", ex.Message);
                }
            }
            return View(managerAccount);
        }

        // GET: ManagerAccounts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.ManagerAccounts == null)
            {
                return NotFound();
            }

            var managerAccount = await _context.ManagerAccounts
                .FirstOrDefaultAsync(m => m.Username == id);
            if (managerAccount == null)
            {
                return NotFound();
            }

            return View(managerAccount);
        }

        // POST: ManagerAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.ManagerAccounts == null)
            {
                return Problem("Entity set 'DataContext.ManagerAccounts'  is null.");
            }
            var managerAccount = await _context.ManagerAccounts.FindAsync(id);
            if (managerAccount != null)
            {
                _context.ManagerAccounts.Remove(managerAccount);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ManagerAccountExists(string id)
        {
          return (_context.ManagerAccounts?.Any(e => e.Username == id)).GetValueOrDefault();
        }
    }
}
