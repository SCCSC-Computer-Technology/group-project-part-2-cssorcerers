using System.ComponentModel.DataAnnotations;

namespace IQSport.Models.AuthModels
{
    public class UserSecurityAnswer
    {

        [Key]
        public int UserSecurityAnswerId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int SecurityQuestionId { get; set; }

        [Required, StringLength(255)]
        public string Answer { get; set; }

        // Navigation properties
        public User User { get; set; }
        public SecurityQuestion SecurityQuestion { get; set; }
    }
}
