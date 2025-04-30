using IQSport.Models.AuthModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static IQSport.Data.DbContext.UsersDbContext;

namespace IQSport.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly UserDbContext _context;


        public AuthController(ILogger<AuthController> logger, UserDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Display the registration page
        // Retrieve security questions from the database to populate a dropdown
        public IActionResult Register()
        {
            var securityQuestions = _context.SecurityQuestions.ToList();

            if (securityQuestions == null || !securityQuestions.Any())
            {
                ViewBag.Error = "No security questions available.";
                return View("RegisterView");
            }

            var selectList = new SelectList(securityQuestions, "QuestionId", "Question");

            if (selectList.Any())
            {
                ViewBag.SecurityQuestions = selectList;
            }
            else
            {
                ViewBag.Error = "No security questions available.";
            }

            return View("RegisterView");
        }

        // Dislay the login page
        public IActionResult Login() => View("LoginView");

        // Display the Index page (-> Login Page)
        public IActionResult Index()
        {
            Console.WriteLine($"User.Identity.IsAuthenticated: {User.Identity.IsAuthenticated}");
            if (User.Identity.IsAuthenticated)
            {
                // Debugging
                Console.WriteLine($"User.Identity.Name: {User.Identity.Name}");
                // End
                
                if (User.Claims != null)
                {
                    foreach (var claim in User.Claims)
                    {
                        Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
                    }
                }
            }

            // If the user is not authenticated, then re-direct to Login Action
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }

            // And if user is authenticated, return the default page.
            return View();
        }

        // Handles User registration process
        [HttpPost]
        public async Task<IActionResult> Register(User model, int securityQuestionId, string securityAnswer)
        {
            // Debuggin:
            Console.WriteLine("Register action started.");
            // End
            
            ModelState.Remove("UserSecurityAnswers");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    // This logs all model state errors - debugging
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }

                // If the model is invalid, then return register page
                return View("RegisterView", model);
            }

            // Checks is a user with the provided email alreasy exists in the DB
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                ViewBag.Error = "Email already registered.";
                return View("RegisterView", model);
            }

            // Hash the user password
            model.Password = HashPassword(model.Password);
            _context.Users.Add(model);

            try
            {
                // Then save the changes
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error (Users): {ex.Message}");
                ViewBag.Error = "A database error occurred during user creation.";
                return View("RegisterView", model);
            }

            if (securityQuestionId == 0 || string.IsNullOrWhiteSpace(securityAnswer))
            {
                ViewBag.Error = "Security question and answer are required.";
                return View("RegisterView", model);
            }

            // Creates a new UserSecurityAnswer object to store user's securty questions
            var userSecurityAnswer = new UserSecurityAnswer
            {
                UserId = model.User_Id,
                SecurityQuestionId = securityQuestionId,
                Answer = securityAnswer
            };
            _context.UserSecurityAnswers.Add(userSecurityAnswer);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error (SecurityAnswer): {ex.Message}");
                ViewBag.Error = "A database error occurred saving security question.";
                return View("RegisterView", model);
            }

            Console.WriteLine("Registration successful. Redirecting to Login.");
            TempData["Success"] = "Registration successful! Please log in.";
            return RedirectToAction("Login");
        }

        // Handles Login Process
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                ViewBag.Error = "Invalid username or password.";
                return View("LoginView");
            }

            if (user.Password != HashPassword(password))
            {
                ViewBag.Error = "Invalid username or password.";
                return View("LoginView");
            }

            // List of claims for the authenticated user.
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.User_Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role ?? "User"),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Sign in the user using cookie-based auth
            await HttpContext.SignInAsync("AuthCookie", principal);

            // Redirect the authenticated user to the Home/Index action
            return RedirectToAction("Index", "Home");
        }

        // Displays the Pasword Request Page
        public IActionResult ResetPwdRequest()
        {
            var securityQuestions = _context.SecurityQuestions.ToList();
            return View(securityQuestions);
        }

        // Handles the password reset process by verifying the user's securoty questions
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string userName, int questionId, string answer)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
                if (user == null)
                {
                    ViewBag.Error = "User not found.";
                    return View("ResetPasswordRequest");
                }

                var securityAnswer = await _context.UserSecurityAnswers
                    .FirstOrDefaultAsync(usa => usa.UserId == user.User_Id && usa.SecurityQuestionId == questionId);

                if (securityAnswer == null || securityAnswer.Answer != answer)
                {
                    ViewBag.Error = "Incorrect answer to security question.";
                    return View("ResetPasswordRequest");
                }

                user.Password = HashPassword("new-password-here");
                await _context.SaveChangesAsync();

                TempData["Success"] = "Your password has been reset.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while processing your request: " + ex.Message;
                return View("ResetPasswordRequest");
            }
        }

        // Handle the user logout process
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AuthCookie");
            return RedirectToAction("Login");
        }

        // Display Access Denied page for anauthenticated users
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ViewUsers()
        {
            var users = await _context.Users.ToListAsync();

            foreach (var user in users)
            {
                user.UserName = user.UserName ?? "Unknown";
                user.Email = user.Email ?? "Unknown";

                // Check if Role is NULL
                if (user.Role == null)
                {
                    user.Role = "Admin"; // Assigned the default role
                }
            }

            return View(users);
        }


        // Handles the provided password using SHA256 algorithm
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
