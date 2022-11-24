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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ISPRO.Web.Authorization;
using System.Linq.Expressions;
using ISPRO.Persistence.Enums;

namespace ISPRO.Web.Controllers
{
    [AuthorizeUserLevel(UserLevelAuth.SUPERUSER)]
    public class SubscriptionsController : Controller
    {
        private readonly DataContext _context;
        private Expression<Func<Subscription, bool>> expression;

        public SubscriptionsController(DataContext context)
        {
            _context = context;
        }

        // GET: Subscriptions
        public async Task<IActionResult> Index()
        {
            if (!User.IsInRole(UserType.ADMIN.ToString()))
                expression = x => x.Project.ProjectManager.Username == User.Identity.Name;
            else
                expression = x => true == true;
            var dataContext = _context.Subscriptions.Include(p => p.Project).Where(expression);
            return View(await dataContext.ToListAsync());
        }

        // GET: Subscriptions/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _context.Subscriptions == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        // GET: Subscriptions/Create
        public IActionResult Create()
        {
            ViewData["ProjectName"] = new SelectList(_context.Projects.ToList(), "Name", "Name");
            return View();
        }

        // POST: Subscriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Bandwidth,Quota,ProjectName,CreationDate,LastUpdate")] Subscription subscription)
        {
            try
            {
                if(new ControllerHelper().ValidateModelStateParentFieldByStrField(ModelState, "Project", "ProjectName", subscription.ProjectName))
                {
                    subscription.Project = await _context.Projects.Where(x => x.Name == subscription.ProjectName).FirstAsync();
                }

                if (ModelState.IsValid)
                {
                    if (_context.Subscriptions.Any(u => u.Name.ToLower() == subscription.Name.Trim().ToLower() && u.ProjectName == subscription.ProjectName))
                    {
                        ModelState.AddModelError("Name", "Name already taken.");
                    }
                    else
                    {
                        subscription.Name = subscription.Name.Trim();
                        _context.Add(subscription);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (ModelException ex)
            {
                ModelState.AddModelError("ModelError", ex.Message);
            }

            ViewData["ProjectName"] = new SelectList(_context.Projects.ToList(), "Name", "Name", subscription.ProjectName);
            return View(subscription);
        }

        // GET: Subscriptions/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _context.Subscriptions == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }
            ViewData["ProjectName"] = new SelectList(_context.Projects.ToList(), "Name", "Name", subscription.ProjectName);
            return View(subscription);
        }

        // POST: Subscriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Bandwidth,Quota,ProjectName,CreationDate,LastUpdate")] Subscription subscription)
        {
            if (id != subscription.Id)
            {
                return NotFound();
            }

            new ReflectionHelper().CopyNullFromOld(await _context.Subscriptions.FindAsync(id), subscription);
            ModelState.Clear();
            TryValidateModel(subscription);

            if (new ControllerHelper().ValidateModelStateParentFieldByStrField(ModelState, "Project", "ProjectName", subscription.ProjectName))
            {
                subscription.Project = await _context.Projects.Where(x => x.Name == subscription.ProjectName).FirstAsync();
            }

            if (ModelState.IsValid)
            {
                if (_context.Subscriptions.Any(u => u.Id != subscription.Id && u.Name.ToLower() == subscription.Name.Trim().ToLower() && u.ProjectName == subscription.ProjectName))
                {
                    ModelState.AddModelError("Name", "Name already taken.");
                }
                else
                {
                    try
                    {
                        _context.Entry(_context.Subscriptions.Where(x => x.Id.Equals(id)).FirstOrDefault()).State = EntityState.Detached;
                        _context.Update(subscription);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!SubscriptionExists(subscription.Name))
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
            }
            ViewData["ProjectName"] = new SelectList(_context.Projects.ToList(), "Name", "Name", subscription.ProjectName);
            return View(subscription);
        }

        // GET: Subscriptions/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || _context.Subscriptions == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        // POST: Subscriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Subscriptions == null)
            {
                return Problem("Entity set 'DataContext.Subscriptions'  is null.");
            }
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription != null)
            {
                _context.Subscriptions.Remove(subscription);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubscriptionExists(string id)
        {
          return (_context.Subscriptions?.Any(e => e.Name == id)).GetValueOrDefault();
        }
    }
}
