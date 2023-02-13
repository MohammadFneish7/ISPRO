using ISPRO.Persistence.Context;
using ISPRO.Persistence.Enums;
using ISPRO.Web.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(12);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options =>
        {
            options.LoginPath = new PathString("/Authentication/Login");
            options.AccessDeniedPath = new PathString("/Authentication/Denied");
        });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.AdminUserPolicy,
        policy => policy.Requirements.Add(new UserLevelRequirement(UserLevelAuth.ADMIN)));
    options.AddPolicy(Policies.SuperUserPolicy,
        policy => policy.Requirements.Add(new UserLevelRequirement(UserLevelAuth.SUPERUSER)));
    options.AddPolicy(Policies.AuthenticatedUserPolicy,
       policy => policy.Requirements.Add(new UserLevelRequirement(UserLevelAuth.AUTHENTICATED)));

});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAuthorizationHandler, UserLevelRequirementHandler>();

builder.Services.AddSingleton(new DataContext());

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
