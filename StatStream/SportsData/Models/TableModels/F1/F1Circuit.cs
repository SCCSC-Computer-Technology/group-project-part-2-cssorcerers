using System.ComponentModel.DataAnnotations;

namespace SportsData.Models
{
    public class F1Circuit
    {
        [Required]
        [Key]
        public int CircuitID { get; set; }
        [Required]
        [StringLength(50)]
        public string? CircuitRef { get; set; }
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        [Required]
        [StringLength(50)]
        public string? Location { get; set; }
        [Required]
        [StringLength(50)]
        public string? Country { get; set; }
        [StringLength(100)]
        public string? Url { get; set; }
    }
}
