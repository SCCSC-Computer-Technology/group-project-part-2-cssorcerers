/*
 * GROUP PROJECT #2
 * AUTH CONTROLLER
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportIQ.Data;
using SportIQ.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SportIQ.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor to Initialize the database context
        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Easy way - Show Register and Login Views
        public IActionResult Register() => View("RegisterView");
        public IActionResult Login() => View("LoginView");


        // HomeView redirection logic, ensure user is autenticated
        public IActionResult Index()
        {
            Console.WriteLine($"User.Identity.IsAuthenticated: {User.Identity.IsAuthenticated}");
            if (User.Identity.IsAuthenticated)
            {
                Console.WriteLine($"User.Identity.Name: {User.Identity.Name}");
                if (User.Claims != null)
                {
                    foreach (var claim in User.Claims)
                    {
                        Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
                    }
                }
            }

            // Redirect to loginView if use is not authenticated
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }
            return View();
        }


        // Registering a new user
        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            if (!ModelState.IsValid) return View("RegisterView", model);

            // Check if email is already taken
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                ViewBag.Error = "Email already registered.";
                return View("RegisterView", model);
            }

            // Hash the password before saving it
            model.Password = HashPassword(model.Password);
            _context.Users.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Registration successful! Please log in.";
            return RedirectToAction("Login");
        }


        // User Login
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            // Validates username and password
            if (user == null || user.Password != HashPassword(password)) // Hash the entered password
            {
                ViewBag.Error = "Invalid username or password.";
                return View("LoginView");
            }

            // Creates auth claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),

                // Note: If any of you want to extend roles, you can do that here
                new Claim(ClaimTypes.Role, "User")
            };

            // Creates claims identity and sign-in
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Sign in user with cookie authentication
            await HttpContext.SignInAsync("AuthCookie", principal);

            return RedirectToAction("Index", "Home"); // Corrected redirect
        }


        // Logout Funtionality
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AuthCookie");

            // Redirect to HomeView after successful login
            return RedirectToAction("Login");
        }


        // Handling unauthorized access
        public IActionResult AccessDenied()
        {
            // Handle access denied
            return View();
        }

        // ---------------------------------- TEMP -------------------------------- //
        // View Users that are protected with [Authorize] to require authentication
        [Authorize]
        public async Task<IActionResult> ViewUsers()
        {
            var users = await _context.Users.ToListAsync(); // Get all users from the database
            return View(users); // Pass the list of users to the view
        }
        // ---------------------------------- TEMP -------------------------------- //

        // Secure password hashing
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
