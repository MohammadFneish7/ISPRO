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
using Microsoft.AspNetCore.Authorization;
using ISPRO.Web.Authorization;
using ISPRO.Persistence.Enums;

namespace ISPRO.Web.Controllers
{
    [AuthorizeUserLevel(UserLevelAuth.SUPERUSER)]
    public class CashPaymentsController : Controller
    {
        private readonly DataContext _context;

        public CashPaymentsController(DataContext context)
        {
            _context = context;
        }

        // GET: CashPayments
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.CashPayments.Include(p => p.UserAccount);
            return View(await dataContext.ToListAsync());
        }

        // GET: CashPayments/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _context.CashPayments == null)
            {
                return NotFound();
            }

            var cashPayment = await _context.CashPayments
                .Include(p => p.UserAccount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cashPayment == null)
            {
                return NotFound();
            }

            return View(cashPayment);
        }

        // GET: CashPayments/Create
        public IActionResult Create()
        {
            ViewData["UserAccountName"] = new SelectList(_context.UserAccounts, "Username", "Username");
            return View();
        }

        // POST: CashPayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserAccountName,PaymentDate,RechargePeriod,Ammount,Currency,CreationDate,LastUpdate")] CashPayment cashPayment)
        {
            try
            {
                if(new ControllerHelper().ValidateModelStateParentFieldByStrField(ModelState, "UserAccount", "UserAccountName", cashPayment.UserAccountName))
                {
                    cashPayment.UserAccount = await _context.UserAccounts.Where(x => x.Username == cashPayment.UserAccountName).FirstAsync();
                }

                if (ModelState.IsValid)
                {
                    _context.Add(cashPayment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (ModelException ex)
            {
                ModelState.AddModelError("ModelError", ex.Message);
            }

            ViewData["UserAccountName"] = new SelectList(_context.UserAccounts, "Username", "Username", cashPayment.UserAccountName);
            return View(cashPayment);
        }

        // GET: CashPayments/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _context.CashPayments == null)
            {
                return NotFound();
            }

            var cashPayment = await _context.CashPayments.FindAsync(id);
            if (cashPayment == null)
            {
                return NotFound();
            }
            ViewData["UserAccountName"] = new SelectList(_context.UserAccounts, "Username", "Username", cashPayment.UserAccountName);
            return View(cashPayment);
        }

        // POST: CashPayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserAccountName,PaymentDate,RechargePeriod,Ammount,Currency,CreationDate,LastUpdate")] CashPayment cashPayment)
        {
            if (id != cashPayment.Id)
            {
                return NotFound();
            }

            new ReflectionHelper().CopyNullFromOld(await _context.CashPayments.FindAsync(id), cashPayment);
            ModelState.Clear();
            TryValidateModel(cashPayment);

            if (new ControllerHelper().ValidateModelStateParentFieldByStrField(ModelState, "UserAccount", "UserAccountName", cashPayment.UserAccountName))
            {
                cashPayment.UserAccount = await _context.UserAccounts.Where(x => x.Username == cashPayment.UserAccountName).FirstAsync();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(_context.CashPayments.Where(x => x.Id.Equals(id)).FirstOrDefault()).State = EntityState.Detached;
                    _context.Update(cashPayment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CashPaymentExists(cashPayment.Id))
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
            ViewData["UserAccountName"] = new SelectList(_context.UserAccounts, "Username", "Username", cashPayment.UserAccountName);
            return View(cashPayment);
        }

        // GET: CashPayments/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || _context.CashPayments == null)
            {
                return NotFound();
            }

            var cashPayment = await _context.CashPayments
                .Include(p => p.UserAccount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cashPayment == null)
            {
                return NotFound();
            }

            return View(cashPayment);
        }

        // POST: CashPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CashPayments == null)
            {
                return Problem("Entity set 'DataContext.CashPayments'  is null.");
            }
            var cashPayment = await _context.CashPayments.FindAsync(id);
            if (cashPayment != null)
            {
                _context.CashPayments.Remove(cashPayment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CashPaymentExists(int id)
        {
          return (_context.CashPayments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
