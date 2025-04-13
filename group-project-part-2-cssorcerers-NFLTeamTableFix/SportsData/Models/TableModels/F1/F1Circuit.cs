using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsData.Models.TableModels.F1
{
    public class F1Circuit
    {
		[Required]
		[Key]
		public int CircuitID { get; set; }
		[Required]
		[StringLength(50)]
		public string? CircuitRef { get; set; }
		[Required]
		[StringLength(50)]
		public string? Name { get; set; }
		[Required]
		[StringLength(50)]
		public string? Location { get; set; }
		[Required]
		[StringLength(50)]
		public string? Country { get; set; }
		[StringLength(100)]
		public string? Url { get; set; }
	}
}
