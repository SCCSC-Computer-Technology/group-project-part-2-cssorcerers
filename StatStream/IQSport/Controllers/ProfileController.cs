using IQSport.Models.AuthModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using static IQSport.Data.DbContext.UsersDbContext;

namespace IQSport.Controllers
{
    public class ProfileController(UserDbContext _context) : Controller
    {
        [Authorize]
        public IActionResult Profile()
        {
            Console.WriteLine("ProfileController.Profile() action called.");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine($"UserId: {userId}");

            var user = _context.Users.FirstOrDefault(u => u.User_Id.ToString() == userId);

            if (user == null)
            {
                Console.WriteLine("User not found.");
                return NotFound();
            }

            var securityQuestions = new SelectList(_context.SecurityQuestions, "QuestionId", "Question");
            ViewBag.SecurityQuestions = securityQuestions; // Ensure this line is present!

            Console.WriteLine("Returning ProfileView.");
            return View("ProfileView", user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(User updateUser)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Users.FindAsync(updateUser.User_Id);

                if (existingUser != null)
                {
                    existingUser.SecurityQuestionId = updateUser.SecurityQuestionId;
                    existingUser.SecurityAnswer = updateUser.SecurityAnswer;

                    _context.Entry(existingUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified; // Explicitly mark as modified
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Profile", "Profile");
                }
                else
                {
                    // Handle the case where the user wasn't found (shouldn't happen in this scenario)
                    return NotFound();
                }
            }

            var securityQuestions = new SelectList(_context.SecurityQuestions, "QuestionId", "Question");
            ViewBag.SecurityQuestions = securityQuestions;
            return View("ProfileView", updateUser);
        }

        [Authorize]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Home"); // Or some error page
            }

            var userToDelete = await _context.Users.FindAsync(int.Parse(userId)); // Assuming User_Id is an int

            if (userToDelete != null)
            {
                _context.Users.Remove(userToDelete);
                await _context.SaveChangesAsync();

                // Sign the user out using Cookie Authentication
                await HttpContext.SignOutAsync();

                return RedirectToAction("Index", "Home"); // Redirect to the home page after deletion
            }
            else
            {
                return NotFound(); // User not found (shouldn't happen if they are logged in)
            }
        }
    }
}