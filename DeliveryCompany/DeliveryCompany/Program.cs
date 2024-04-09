using Constants.Authentification;
using DataAccess.DataAccess;
using DeliveryCompany.DataAccess.Data;
using DeliveryCompany.Services.IServices;
using DeliveryCompany.Services.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Authentification;
using Repository.IRepository;
using Repository.Repository;
using Services.IServices;
using Services.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionStringIdentity = builder.Configuration.GetConnectionString("DefaultConnectionIndentity");
var connectionStringData = builder.Configuration.GetConnectionString("DefaultConnectionData");

builder.Services.AddDbContext<AppIdentityDbAccess>(
    options => options.UseSqlServer(connectionStringIdentity));

builder.Services.AddDbContext<DataAppDbContext>(
    options => options.UseSqlServer(connectionStringData));

builder.Services.AddTransient<ICityRepository, CityRepository>();
builder.Services.AddTransient<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddTransient<IPageDescriptionRepository, PageDescriptionRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();

builder.Services.AddTransient<IPageDescriptionService, PageDescriptionService>();
builder.Services.AddTransient<ICityService, CityService>();
builder.Services.AddTransient<IOrdersService, OrdersService>();

builder.Services.AddIdentity<AppUser, IdentityRole>(
    options =>
    {
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<AppIdentityDbAccess>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Creating the roles if they do not exists

using (var scope = app.Services.CreateScope())
{
    var roleManager =
        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { UserRoles.Admin, UserRoles.DeliveryEmployee, UserRoles.User };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<RedirectMiddleware>();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
