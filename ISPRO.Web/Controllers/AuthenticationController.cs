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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using ISPRO.Web.Models;

namespace ISPRO.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly DataContext _context;

        public AuthenticationController(DataContext context)
        {
            _context = context;
        }
        
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Denied()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username,Password")] LoginRequest loginRequest)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    AbstractUser? user;

                    if (loginRequest.Username.Trim().EndsWith("@admins.com", StringComparison.InvariantCultureIgnoreCase))
                    {
                        user = _context.AdminAccounts.FirstOrDefault(u => u.Username.ToLower() == loginRequest.Username.Trim().ToLower() && u.Password == CryptoHelper.ComputeSHA256Hash(loginRequest.Password));
                    }else if (loginRequest.Username.Trim().EndsWith("@managers.com", StringComparison.InvariantCultureIgnoreCase))
                    {
                        user = _context.ManagerAccounts.FirstOrDefault(u => u.Username.ToLower() == loginRequest.Username.Trim().ToLower() && u.Password == CryptoHelper.ComputeSHA256Hash(loginRequest.Password));
                    }
                    else
                    {
                        user = _context.UserAccounts.FirstOrDefault(u => u.Username.ToLower() == loginRequest.Username.Trim().ToLower() && u.Password == CryptoHelper.ComputeSHA256Hash(loginRequest.Password));
                    }

                    if(user != null)
                    {
                        if(user.IsExpired)
                            throw new ModelException("User expired!! Please contact your administrator to reactivate your user account.");

                        if (!user.Active)
                            throw new ModelException("User deavtivated!! Please contact your administrator to reactivate your user account.");

                        var claims = new List<Claim>
                        { 
                            new Claim(ClaimTypes.Name, user.Username),
                            new Claim(ClaimTypes.Role, user.UserType.ToString()),
                        };

                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            AllowRefresh = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(10),
                            IsPersistent = true,
                            IssuedUtc = DateTimeOffset.UtcNow
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        return Redirect("/Home");
                    }

                    throw new ModelException("Invalid Username or Password.");
                }
            }
            catch (ModelException ex)
            {
                ModelState.AddModelError("ModelError", ex.Message);
            }

            return View(loginRequest);
        }

        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Home");
        }
    }
}
