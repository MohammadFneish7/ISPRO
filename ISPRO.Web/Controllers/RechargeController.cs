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
using System.Linq.Expressions;
using ISPRO.Persistence.Enums;
using System.IO;

namespace ISPRO.Web.Controllers
{
    [AuthorizeUserLevel(UserLevelAuth.AUTHENTICATED)]
    public class RechargeController : Controller
    {
        private readonly DataContext _context;
        private Expression<Func<UserAccount, bool>> expression;

        public RechargeController(DataContext context)
        {
            _context = context;
        }

        // GET: PrePaidCards
        public async Task<IActionResult> Recharge()
        {
            setFilterExpression();
            ViewData["Users"] = new SelectList(_context.UserAccounts.Where(expression).ToList(), "Username", "Username");
            return View();
        }

        private void setFilterExpression()
        {
            if (!User.IsInRole(UserType.ADMIN.ToString()))
                expression = x => x.Project.ProjectManager.Username == User.Identity.Name;
            else
                expression = x => true == true;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Recharge([Bind("Username,RechargeCode")] RechargeRequest rechargeRequest)
        {
            try
            {
                rechargeRequest.ModelSuccess = string.Empty;

                if (ModelState.IsValid)
                {
                    if (User.IsInRole(UserType.USER_ACCOUNT.ToString()))
                        rechargeRequest.Username = User.Identity?.Name;

                    if (string.IsNullOrEmpty(rechargeRequest.Username))
                        throw new ModelException("User is required");

                    var user = await _context.UserAccounts.Include(x=>x.Subscription).Include(x=>x.Subscription.Project).Where(x => x.Username == rechargeRequest.Username).FirstOrDefaultAsync();
                    if (user == null)
                        throw new ModelException("Invalid user selected");

                    if(!user.Active)
                        throw new ModelException("Operation denied. User is in-active!");

                    if (user.IsExpired)
                        throw new ModelException("Operation denied. User has expired!");

                    if (rechargeRequest.RechargeCode != null) { 
                        string[] parts = rechargeRequest.RechargeCode.Split(" ");
                        var rechargecode = string.Join("", parts).Trim();
                        var card = await _context.PrePaidCards.Where(x => x.Subscription.ProjectName == user.Subscription.ProjectName && x.Id == rechargecode).FirstOrDefaultAsync();
                        if(card != null)
                        {
                            if (card.IsExpired)
                            {
                                throw new ModelException("Card is expired!");
                            }
                            
                            if (card.IsConsumed)
                            {
                                throw new ModelException("Card is already consumed!");
                            }

                            if (user.IsValid)
                                user.ValidityDate = (user.ValidityDate!=null ? user.ValidityDate.Value : DateTime.Now).AddDays(card.RechargePeriod);
                            else
                                user.ValidityDate = DateTime.Now.AddDays(card.RechargePeriod);

                            card.Consumer = user;
                            card.ConsumptionDate = DateTime.Now;
                            await _context.SaveChangesAsync();
                            rechargeRequest.ModelSuccess = $"Card recharged successfully, The user account is now valid until '{user.ValidityDate.Value.ToString("dd/MM/yyyy")}'";
                        }
                        else
                        {
                            throw new ModelException("Invalid recharge code.");
                        }
                    }
                    else
                    {
                        throw new ModelException("RechargeCode is required");
                    }
                }
            }
            catch (ModelException ex)
            {
                ModelState.AddModelError("ModelError", ex.Message);
            }

            setFilterExpression();
            ViewData["Users"] = new SelectList(_context.UserAccounts.Where(expression).ToList(), "Username", "Username");
            return View(rechargeRequest);
        }

    }
}
