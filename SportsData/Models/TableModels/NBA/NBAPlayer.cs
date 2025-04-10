using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsData.Models.TableModels
{
    public class NBAPlayer
    {
        [Key, Column("PlayerID")]
        public int ID { get; set; }

        public int? TeamID { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }  
    }
}
