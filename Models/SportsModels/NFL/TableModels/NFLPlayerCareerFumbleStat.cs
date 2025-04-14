using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IQSport.Models.NFL.TableModels
{
    public class NFLPlayerCareerFumbleStat
    {
        [Required]
        [Column("PlayerID")]
        public int ID { get; set; }

        public int Fumbles { get; set; }
    }
}
