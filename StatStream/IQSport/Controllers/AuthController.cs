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

        public IActionResult Login() => View("LoginView");

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

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User model, int securityQuestionId, string securityAnswer)
        {
            Console.WriteLine("Register action started.");
            ModelState.Remove("UserSecurityAnswers");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }
                return View("RegisterView", model);
            }

            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                ViewBag.Error = "Email already registered.";
                return View("RegisterView", model);
            }

            model.Password = HashPassword(model.Password);
            _context.Users.Add(model);

            try
            {
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

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.User_Id.ToString()), // Add this line
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role ?? "User"),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("AuthCookie", principal);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ResetPwdRequest()
        {
            var securityQuestions = _context.SecurityQuestions.ToList();
            return View(securityQuestions);
        }

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

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AuthCookie");
            return RedirectToAction("Login");
        }

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
                    user.Role = "Admin"; // Assign default role
                }
            }

            return View(users);
        }


        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
