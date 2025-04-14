using System.ComponentModel.DataAnnotations;

namespace IQSport.Models.AuthModels
{
    public class SecurityQuestion
    {
        [Key]
        public int QuestionId { get; set; }

        [Required]
        public string Question { get; set; }

        public ICollection<UserSecurityAnswer> UserSecurityAnswers { get; set; } = new List<UserSecurityAnswer>();

    }
}
