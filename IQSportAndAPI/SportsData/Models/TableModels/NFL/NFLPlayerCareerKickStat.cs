using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsData.Models
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

        public NFLPlayer Player { get; set; }
    }
}
