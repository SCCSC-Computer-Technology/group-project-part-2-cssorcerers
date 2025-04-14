using System.ComponentModel.DataAnnotations;

namespace IQSport.Models.SportsModels.F1.Models
{
    public class F1Status
    {
        [Required]
        [Key]
        public int StatusID { get; set; }
        [Required]
        [StringLength(50)]
        public string? Status { get; set; }
    }
}
