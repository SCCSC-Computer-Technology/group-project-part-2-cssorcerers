/*
 * GROUP PROJECT #2
 * USER MODEL
 */

using System.ComponentModel.DataAnnotations;

namespace SportIQ.Models
{
    public class User
    {
        [Key]
        public int User_Id { get; set; }

        [Required, StringLength(100)]
        public string UserName { get; set; }

        [Required, StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(255)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    }
}
