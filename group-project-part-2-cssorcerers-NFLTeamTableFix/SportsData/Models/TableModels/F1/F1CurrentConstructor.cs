using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsData.Models.TableModels.F1
{
    public class F1CurrentConstructor
    {
		[Required]
		[Key]
		public int ConstructorID { get; set; }
		[Required]
		[StringLength(50)]
		public string? TeamName { get; set; }
		[Required]
		[StringLength(50)]
		public string? Principal { get; set; }
		[Required]
		[StringLength(50)]
		public string? Driver1 { get; set; }
		[Required]
		public int? D1Num { get; set; }
		[Required]
		[StringLength(50)]
		public string? Driver2 { get; set; }
		[Required]
		public int? D2Num { get; set; }
		public int? Championships { get; set; }
		public string? Base { get; set; }
		public string? PowerUnit { get; set; }
	}
}
