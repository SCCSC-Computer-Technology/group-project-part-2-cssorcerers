using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IQSport.Models.NFL.TableModels
{
    public class NFLPlayerCareerRushStat
    {
        [Required]
        [Column("PlayerID")]
        public int ID { get; set; }
        public int RushAttempts { get; set; }
        public int RushYards { get; set; }
        public int RushTouchdowns { get; set; }
        public int RushFirstdowns { get; set; }
    }
}
