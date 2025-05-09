using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementMVC.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure Identity with auto-confirmation and login timeout
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    // Auto-confirm account (no email confirmation required)
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    
    // Set cookie expiration for login timeout
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// Configure cookie policies for login timeout
builder.Services.ConfigureApplicationCookie(options => {
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(1); // Session timeout after 1 hour
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true; // Reset the cookie expiration if the user returns
});

builder.Services.AddControllersWithViews();

// Configure HttpsRedirection with default options or minimal configuration
builder.Services.AddHttpsRedirection(options =>
{
    // Default configuration is often sufficient.
    // If a specific port needs to be set, it would be done here,
    // but we are reverting to a more default state.
    // options.HttpsPort = 7189; // Example if a specific port was needed
});

// Option 2: Configure Forwarded Headers (if running behind a reverse proxy)
/*
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | 
        Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
    // If your proxy is not on localhost, you might need to clear known proxies/networks:
    // options.KnownProxies.Clear();
    // options.KnownNetworks.Clear();
});
*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// If you configured ForwardedHeadersOptions, add the middleware here:
// Make sure UseForwardedHeaders is one of the first middleware components.
/*
app.UseForwardedHeaders();
*/

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
