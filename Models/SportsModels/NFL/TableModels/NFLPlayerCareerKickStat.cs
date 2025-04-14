using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IQSport.Models.NFL.TableModels
{
    public class NFLPlayerCareerKickStat
    {
        [Required]
        [Column("PlayerID")]
        public int ID { get; set; }

        public int FieldGoalsMade { get; set; }
        public int FieldGoalAttempts { get; set; }
        public int ExtraPointsMade { get; set; }
        public int ExtraPointAttempts { get; set; }
    }
}
