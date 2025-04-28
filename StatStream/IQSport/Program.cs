using Microsoft.EntityFrameworkCore;
using static IQSport.Data.DbContext.UsersDbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Configure the Entity Framework Core
builder.Services.AddDbContext<UserDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
.EnableSensitiveDataLogging()
.LogTo(Console.WriteLine, LogLevel.Information));

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

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

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

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ----- DEBUGGING --------- //
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request Path: {context.Request.Path}");

    if (context.Request.Path == "/debug/routes")
    {
        await context.Response.WriteAsync("Debug Route Active");
        // This ensures there is no further processing
        return;
    }

    await next();
});

app.Run();
