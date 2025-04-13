using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsData.Models.TableModels.F1
{
    public class F1Status
    {
		[Required]
		[Key]
		public int StatusID { get; set; }
		[Required]
		[StringLength(50)]
		public string? Status { get; set; }
	}
}
