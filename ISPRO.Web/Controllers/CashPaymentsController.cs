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
using System.Linq.Expressions;
using Org.BouncyCastle.Asn1.X509;

namespace ISPRO.Web.Controllers
{
    [AuthorizeUserLevel(UserLevelAuth.SUPERUSER)]
    public class CashPaymentsController : Controller
    {
        private readonly DataContext _context;
        private Expression<Func<CashPayment, bool>> expression;

        public CashPaymentsController(DataContext context)
        {
            _context = context;
        }

        // GET: CashPayments
        public async Task<IActionResult> Index()
        {
            if (!User.IsInRole(UserType.ADMIN.ToString()))
                expression = x => x.UserAccount.Project.ProjectManager.Username == User.Identity.Name;
            else
                expression = x => true == true;
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
            ViewData["UserAccountName"] = new SelectList(_context.UserAccounts.ToList(), "Username", "Username");
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
                    var user = await _context.UserAccounts.Where(x => x.Username == cashPayment.UserAccountName).FirstAsync();
                    
                    if (user == null)
                        throw new ModelException("Invalid user selected");

                    if (!user.Active)
                        throw new ModelException("Operation denied. User is in-active!");

                    if (user.IsExpired)
                        throw new ModelException("Operation denied. User has expired!");

                    cashPayment.UserAccount = user;

                }

                if (ModelState.IsValid)
                {
                    if (cashPayment.UserAccount.IsValid)
                        cashPayment.UserAccount.ValidityDate = (cashPayment.UserAccount.ValidityDate != null ? cashPayment.UserAccount.ValidityDate.Value : DateTime.Now).AddDays(cashPayment.RechargePeriod);
                    else
                        cashPayment.UserAccount.ValidityDate = DateTime.Now.AddDays(cashPayment.RechargePeriod);

                    _context.Add(cashPayment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (ModelException ex)
            {
                ModelState.AddModelError("ModelError", ex.Message);
            }

            ViewData["UserAccountName"] = new SelectList(_context.UserAccounts.ToList(), "Username", "Username", cashPayment.UserAccountName);
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
