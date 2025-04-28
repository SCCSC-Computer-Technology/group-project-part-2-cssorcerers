using System.ComponentModel.DataAnnotations;

namespace SportsData.Models
{
    public class F1Constructor
    {
        [Required]
        [Key]
        public int ConstructorID { get; set; }
        [Required]
        [StringLength(50)]
        public string? ConstructorRef { get; set; }
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        [Required]
        [StringLength(50)]
        public string? Nationality { get; set; }
        [StringLength(100)]
        public string? Url { get; set; }
    }
}
