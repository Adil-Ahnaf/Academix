using AspNetCore.Identity.Dapper;
using AspNetCore.Identity.Dapper.Models;
using Microsoft.AspNetCore.Identity;
using DataAccessLayer.SqlDb;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ ";

    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
}).AddDapperStores(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("Portal_DbConnection");
    options.DbSchema = "dbo";
}).AddDefaultTokenProviders();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "UserAuthenticationCookieAuth";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


ResolveServiceType(builder.Services);

// Add services to the container.
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
app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();


void ResolveServiceType(IServiceCollection services)
{
    services.AddSingleton(new DbConnectionInfo { Key = "Portal_DbConnection" });
    services.AddSingleton<IDataAccess, SqlDapperDataAccess>();
}

