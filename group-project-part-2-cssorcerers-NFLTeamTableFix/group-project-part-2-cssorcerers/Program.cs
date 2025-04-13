using Microsoft.EntityFrameworkCore;
using SportIQ.Data;
using SportsData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configures the Entity Framework Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


string root = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.Length - 50) + "SportsData";

builder.Services.AddDbContext<SportsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SportsDB").Replace("%ROOT%", root)));

// Authentication and Cookie settings
builder.Services.AddAuthentication("AuthCookie")
    .AddCookie("AuthCookie", options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";

        // This is the cookie name - I have created
        options.Cookie.Name = "AuthCookie";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite = SameSiteMode.Lax;

        // This is expiration time
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });


// Required for session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    // Seesion timeout
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configures the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Correct middleware order
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ----- TESTING ------ //
//name: "viewusers",
//pattern: "Auth/ViewUsers",
//defaults: new { controller = "Auth", action = "ViewUsers" });


app.Run();
