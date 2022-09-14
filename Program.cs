﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using SurfsUp.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SurfsUpContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SurfsUpContext") ?? throw new InvalidOperationException("Connection string 'SurfsUpContext' not found.")));


// Make Program.cs use the right connectionstring for identity db on startup

var connectionString = builder.Configuration.GetConnectionString("IdentitySurfsUpContextConnection");
builder.Services.AddDbContext<IdentitySurfsUpContext>(x => x.UseSqlServer(connectionString));

// Add Identity as a service, and use AppUser as class for users.
// Also add Roles via IdentityRole Class
// Use IdentitySurfsUpContext for DB context

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<IdentitySurfsUpContext>();

//Add Authorization as a service
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});



// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllers(
    options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var Services = scope.ServiceProvider;
    SeedData.Initialize(Services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRequestLocalization("fr-FR");

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Rental}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
