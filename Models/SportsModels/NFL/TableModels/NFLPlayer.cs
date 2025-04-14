using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IQSport.Models.NFL.TableModels
{
    public class NFLPlayer
    {
        [Required]
        [Column("PlayerID")]
        public int ID { get; set; }
        public int? TeamID { get; set; }
        [StringLength(100)]
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
    }
}
