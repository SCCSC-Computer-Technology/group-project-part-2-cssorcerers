using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsData.Models.TableModels
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
