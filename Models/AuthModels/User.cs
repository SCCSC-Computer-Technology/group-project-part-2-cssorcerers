using System.ComponentModel.DataAnnotations;

namespace IQSport.Models.AuthModels
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

        public bool Is_Active { get; set; }

        public string Role { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public ICollection<UserSecurityAnswer> UserSecurityAnswers { get; set; }

        public int SecurityQuestionId { get; set; }
        public string SecurityAnswer { get; set; }
    }
}
