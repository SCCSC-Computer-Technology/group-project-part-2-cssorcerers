using System.ComponentModel.DataAnnotations;
namespace SportsData.Models
{
    public class F1Driver
    {
        [Required]
        [Key]
        public int DriverID { get; set; }
        [Required]
        [StringLength(50)]
        public string? DriverRef { get; set; }
        public int? Number { get; set; }
        [StringLength(50)]
        public string? Code { get; set; }
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        public DateTime? Dob { get; set; }
        [Required]
        [StringLength(50)]
        public string? Nationality { get; set; }
        [StringLength(100)]
        public string? Url { get; set; }
    }
}
