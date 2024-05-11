using Constants.Authentification;
using DataAccess.DataAccess;
using DeliveryCompany.DataAccess.Data;
using DeliveryCompany.Services.IServices;
using DeliveryCompany.Services.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models.Authentification;
using Repository.IRepository;
using Repository.Repository;
using Serilog;
using Serilog.Filters;
using Serilog.Sinks.MSSqlServer;
using Services.IServices;
using Services.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionStringIdentity = builder.Configuration.GetConnectionString("DefaultConnectionIndentity");
var connectionStringData = builder.Configuration.GetConnectionString("DefaultConnectionData");


Log.Logger = new LoggerConfiguration()
   .MinimumLevel.Information()
   .WriteTo.Console()
   .WriteTo.File("Logs/DeliveryCompanyLogs.txt", rollingInterval: RollingInterval.Day)
   .CreateLogger();


builder.Host.UseSerilog();

builder.Services.AddDbContext<AppIdentityDbAccess>(
    options => options.UseSqlServer(connectionStringIdentity));

builder.Services.AddDbContext<DataAppDbContext>(
    options => options.UseSqlServer(connectionStringData));

//Repos
builder.Services.AddTransient<ICityRepository, CityRepository>();
builder.Services.AddTransient<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddTransient<IPageDescriptionRepository, PageDescriptionRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddTransient<IDeliveryRepository, DeliveryRepository>();

//Services
builder.Services.AddTransient<IPageDescriptionService, PageDescriptionService>();
builder.Services.AddTransient<ICityService, CityService>();
builder.Services.AddTransient<IOrdersService, OrdersService>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IDeliveryCarsService, DeliveryCarService>();
builder.Services.AddTransient<IDeliveryService, DeliveryService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();

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

app.Lifetime.ApplicationStopped.Register(() => Log.CloseAndFlush());
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

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var adminUser = await userManager.FindByNameAsync("admin");
    if (adminUser == null)
    {
        var newAdmin = new AppUser
        {
            Name = "admin",
            Email = "admin@admin.com",
            UserName = "admin",
            Address = "admin",
        };
        var result = await userManager.CreateAsync(newAdmin, "123456789");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdmin, UserRoles.Admin);
        }
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
