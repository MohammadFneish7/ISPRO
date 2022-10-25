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
using ISPRO.Web.Models;
using ISPRO.Web.Authorization;

namespace ISPRO.Web.Controllers
{
    [AuthorizeUserLevel(UserLevelAuth.SUPERUSER)]
    public class PrePaidCardsController : Controller
    {
        private readonly DataContext _context;

        public PrePaidCardsController(DataContext context)
        {
            _context = context;
        }

        // GET: PrePaidCards
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.PrePaidCards.Include(p => p.Consumer).Include(p => p.Subscription);
            return View(await dataContext.ToListAsync());
        }

        // GET: PrePaidCards/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.PrePaidCards == null)
            {
                return NotFound();
            }

            var prePaidCard = await _context.PrePaidCards
                .Include(p => p.Consumer).Include(p => p.Subscription)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prePaidCard == null)
            {
                return NotFound();
            }

            return View(prePaidCard);
        }

        // GET: PrePaidCards/Create
        public IActionResult Create()
        {
            ViewData["ConsumerName"] = new SelectList(_context.UserAccounts, "Username", "Username");
            ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "Id", "Name");
            ViewData["GeneratedId"] = PrePaidCard.GenerateId();
            return View();
        }

        // POST: PrePaidCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubscriptionId,ExpiryDate,RechargePeriod,Price,Currency,CreationDate,LastUpdate")] PrePaidCard prePaidCard)
        {
            try
            {
                if (new ControllerHelper().ValidateModelStateParentFieldByStrField(ModelState, "Subscription", "SubscriptionId", prePaidCard.SubscriptionId))
                {
                    prePaidCard.Subscription = await _context.Subscriptions.Where(x => x.Id == prePaidCard.SubscriptionId).FirstAsync();
                }

                if (ModelState.IsValid)
                {
                    _context.Add(prePaidCard);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (ModelException ex)
            {
                ModelState.AddModelError("ModelError", ex.Message);
            }

            ViewData["ConsumerName"] = new SelectList(_context.UserAccounts, "Username", "Username", prePaidCard.ConsumerName);
            ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "Id", "Name", prePaidCard.SubscriptionId);
            return View(prePaidCard);
        }

        // GET: PrePaidCards/Generate
        public IActionResult Generate()
        {
            ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "Id", "Name");
            return View();
        }

        // POST: PrePaidCards/Generate
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Generate([Bind("SubscriptionId,ExpiryDate,RechargePeriod,Price,Currency,NumberOfCards")] PrepaidCardsGenerationRequest prepaidCardsGenerationRequest)
        {
            try
            {
                if (new ControllerHelper().ValidateModelStateParentFieldByStrField(ModelState, "Subscription", "SubscriptionId", prepaidCardsGenerationRequest.SubscriptionId))
                {
                    prepaidCardsGenerationRequest.Subscription = await _context.Subscriptions.Where(x => x.Id == prepaidCardsGenerationRequest.SubscriptionId).FirstAsync();
                }

                if (ModelState.IsValid)
                {
                    for(int i=0; i <= prepaidCardsGenerationRequest.NumberOfCards; i++)
                    {
                        _context.Add(new PrePaidCard()
                        {
                            Id = PrePaidCard.GenerateId(),
                            SubscriptionId = prepaidCardsGenerationRequest.SubscriptionId,
                            ExpiryDate = prepaidCardsGenerationRequest.ExpiryDate,
                            RechargePeriod = prepaidCardsGenerationRequest.RechargePeriod,
                            Price = prepaidCardsGenerationRequest.Price,
                            Currency = prepaidCardsGenerationRequest.Currency
                        });
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (ModelException ex)
            {
                ModelState.AddModelError("ModelError", ex.Message);
            }

            ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "Id", "Name", prepaidCardsGenerationRequest.SubscriptionId);
            return View(prepaidCardsGenerationRequest);
        }

        // GET: PrePaidCards/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.PrePaidCards == null)
            {
                return NotFound();
            }

            var prePaidCard = await _context.PrePaidCards.FindAsync(id);
            if (prePaidCard == null)
            {
                return NotFound();
            }
            ViewData["ConsumerName"] = new SelectList(_context.UserAccounts, "Username", "Username", prePaidCard.ConsumerName);
            ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "Id", "Name", prePaidCard.SubscriptionId);
            return View(prePaidCard);
        }

        // POST: PrePaidCards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,SubscriptionId,ExpiryDate,RechargePeriod,RechargePeriod,Price,Currency,CreationDate,LastUpdate")] PrePaidCard prePaidCard)
        {
            if (id != prePaidCard.Id)
            {
                return NotFound();
            }

            new ReflectionHelper().CopyNullFromOld(await _context.PrePaidCards.FindAsync(id), prePaidCard);
            ModelState.Clear();
            TryValidateModel(prePaidCard);

            if (new ControllerHelper().ValidateModelStateParentFieldByStrField(ModelState, "Subscription", "SubscriptionId", prePaidCard.SubscriptionId))
            {
                prePaidCard.Subscription = await _context.Subscriptions.Where(x => x.Id == prePaidCard.SubscriptionId).FirstAsync();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(_context.PrePaidCards.Where(x => x.Id.Equals(id)).FirstOrDefault()).State = EntityState.Detached;
                    _context.Update(prePaidCard);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrePaidCardExists(prePaidCard.Id))
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
            ViewData["ConsumerName"] = new SelectList(_context.UserAccounts, "Username", "Username", prePaidCard.ConsumerName);
            ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "Id", "Name", prePaidCard.SubscriptionId);
            return View(prePaidCard);
        }

        // GET: PrePaidCards/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.PrePaidCards == null)
            {
                return NotFound();
            }

            var prePaidCard = await _context.PrePaidCards
                .Include(p => p.Consumer).Include(p => p.Subscription)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prePaidCard == null)
            {
                return NotFound();
            }

            return View(prePaidCard);
        }

        // POST: PrePaidCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.PrePaidCards == null)
            {
                return Problem("Entity set 'DataContext.PrePaidCards'  is null.");
            }
            var prePaidCard = await _context.PrePaidCards.FindAsync(id);
            if (prePaidCard != null)
            {
                _context.PrePaidCards.Remove(prePaidCard);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrePaidCardExists(string id)
        {
          return (_context.PrePaidCards?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
