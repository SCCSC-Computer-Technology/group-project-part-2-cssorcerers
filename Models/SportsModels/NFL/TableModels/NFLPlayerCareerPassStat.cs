using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IQSport.Models.NFL.TableModels
{
    public class NFLPlayerCareerPassStat
    {
        [Required]
        [Column("PlayerID")]
        public int ID { get; set; }

        public int CompletePasses { get; set; }
        public int PassAttempts { get; set; }
        public int PassingYards { get; set; }
        public int TouchdownPasses { get; set; }
        public int Interceptions { get; set; }
        public int LongestPass { get; set; }
    }
}
