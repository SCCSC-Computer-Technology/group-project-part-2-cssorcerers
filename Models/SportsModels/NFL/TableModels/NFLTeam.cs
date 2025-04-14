using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IQSport.Models.SportsModels.NFL.TableModels
{
    public class NFLTeam
    {
        [Required]
        [Column("TeamID")]
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        [Column("TeamName")]
        public string Name { get; set; }
    }
}