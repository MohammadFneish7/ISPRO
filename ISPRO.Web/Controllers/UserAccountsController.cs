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
    [AuthorizeUserLevel(UserLevelAuth.SUPERUSER)]
    public class UserAccountsController : Controller
    {
        private readonly DataContext _context;

        public UserAccountsController(DataContext context)
        {
            _context = context;
        }

        // GET: UserAccounts
        public async Task<IActionResult> Index()
        {
              return _context.UserAccounts != null ? 
                          View(await _context.UserAccounts.Include(u => u.Project).Include(u => u.Subscription).ToListAsync()) :
                          Problem("Entity set 'DataContext.UserAccounts'  is null.");
        }

        // GET: UserAccounts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.UserAccounts == null)
            {
                return NotFound();
            }

            var userAccount = await _context.UserAccounts.Include(u => u.Project).Include(u => u.Subscription)
                .FirstOrDefaultAsync(m => m.Username == id);
            if (userAccount == null)
            {
                return NotFound();
            }

            return View(userAccount);
        }

        // GET: UserAccounts/Create
        public IActionResult Create()
        {
            ViewData["ProjectName"] = new SelectList(_context.Projects, "Name", "Name");
            ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "Id", "Name");
            return View();
        }

        // POST: UserAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,UserType,ProjectName,SubscriptionId,DisplayName,Password,Mobile,Email,ExpiryDate,ValidityDate,Active")] UserAccount userAccount)
        {
            try
            {
                if (new ControllerHelper().ValidateModelStateParentFieldByStrField(ModelState, "Project", "ProjectName", userAccount.ProjectName))
                {
                    userAccount.Project = await _context.Projects.Where(x => x.Name == userAccount.ProjectName).FirstAsync();
                }

                if (new ControllerHelper().ValidateModelStateParentFieldByStrField(ModelState, "Subscription", "SubscriptionId", userAccount.SubscriptionId))
                {
                    userAccount.Subscription = await _context.Subscriptions.Where(x => x.Id == userAccount.SubscriptionId).FirstAsync();
                }

                if (ModelState.IsValid)
                {
                    Regex rgx = new Regex("^[a-zA-Z0-9_]*$");
                    
                    if (_context.UserAccounts.Any(u => u.Username.ToLower() == userAccount.Username.Trim().ToLower() || u.Username.ToLower() == (userAccount.Username.Trim().ToLower() + $"@{userAccount.ProjectName}.com")))
                    {
                        ModelState.AddModelError("Username", "Username already taken.");
                    }
                    else if (!rgx.IsMatch(userAccount.Username.Trim()))
                    {
                        ModelState.AddModelError("Username", "Username could only be alphanumeric with '_'.");
                    }
                    else
                    {
                        userAccount.Username = userAccount.Username.Trim();
                        _context.Add(userAccount);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (ModelException ex)
            {
                ModelState.AddModelError("ModelError", ex.Message);
            }

            ViewData["ProjectName"] = new SelectList(_context.Projects, "Name", "Name", userAccount.ProjectName);
            ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "Id", "Name", userAccount.SubscriptionId);
            return View(userAccount);
        }

        // GET: UserAccounts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.UserAccounts == null)
            {
                return NotFound();
            }

            var userAccount = await _context.UserAccounts.FindAsync(id);
            if (userAccount == null)
            {
                return NotFound();
            }
            ViewData["ProjectName"] = new SelectList(_context.Projects, "Name", "Name", userAccount.ProjectName);
            ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "Id", "Name", userAccount.SubscriptionId);
            return View(userAccount);
        }

        // POST: UserAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Username,UserType,ProjectName,SubscriptionId,DisplayName,Password,Mobile,Email,ExpiryDate,ValidityDate,Active")] UserAccount userAccount)
        {
            if (id != userAccount.Username)
            {
                return NotFound();
            }

            new ReflectionHelper().CopyNullFromOld(await _context.UserAccounts.FindAsync(id), userAccount);
            ModelState.Clear();
            TryValidateModel(userAccount);

            if (new ControllerHelper().ValidateModelStateParentFieldByStrField(ModelState, "Project", "ProjectName", userAccount.ProjectName))
            {
                userAccount.Project = await _context.Projects.Where(x => x.Name == userAccount.ProjectName).FirstAsync();
            }

            if (new ControllerHelper().ValidateModelStateParentFieldByStrField(ModelState, "Subscription", "SubscriptionId", userAccount.SubscriptionId))
            {
                userAccount.Subscription = await _context.Subscriptions.Where(x => x.Id == userAccount.SubscriptionId).FirstAsync();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(_context.UserAccounts.Where(x => x.Username.Equals(id)).FirstOrDefault()).State = EntityState.Detached;
                    _context.Update(userAccount);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserAccountExists(userAccount.Username))
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

            ViewData["ProjectName"] = new SelectList(_context.Projects, "Name", "Name", userAccount.ProjectName);
            ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "Id", "Name", userAccount.SubscriptionId);
            return View(userAccount);
        }

        // GET: UserAccounts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.UserAccounts == null)
            {
                return NotFound();
            }

            var userAccount = await _context.UserAccounts.Include(u => u.Project).Include(u => u.Subscription)
                .FirstOrDefaultAsync(m => m.Username == id);
            if (userAccount == null)
            {
                return NotFound();
            }

            return View(userAccount);
        }

        // POST: UserAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.UserAccounts == null)
            {
                return Problem("Entity set 'DataContext.UserAccounts'  is null.");
            }
            var userAccount = await _context.UserAccounts.FindAsync(id);
            if (userAccount != null)
            {
                _context.UserAccounts.Remove(userAccount);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserAccountExists(string id)
        {
          return (_context.UserAccounts?.Any(e => e.Username == id)).GetValueOrDefault();
        }
    }
}
