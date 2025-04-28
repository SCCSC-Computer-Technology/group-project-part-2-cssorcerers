using System.ComponentModel.DataAnnotations;

namespace SportsData.Models
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
