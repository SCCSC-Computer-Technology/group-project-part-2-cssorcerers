using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IQSport.Models.NFL.TableModels
{
    public class NFLPlayerCareerSackStat
    {
        [Required]
        [Column("PlayerID")]
        public int ID { get; set; }
        public int Sacks { get; set; }
        public int YardsLostToSacks { get; set; }
    }
}
