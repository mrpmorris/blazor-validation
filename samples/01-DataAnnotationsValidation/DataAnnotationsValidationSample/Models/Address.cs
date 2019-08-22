using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataAnnotationsValidationSample.Models
{
	public class Address
	{
		[Required]
		public string Line1 { get; set; }
		public string Line2 { get; set; }
		public string Line3 { get; set; }
		[Required]
		public string Town { get; set; }
		[Required]
		public string Area { get; set; }
		[Required]
		public string Country { get; set; }
		[Required]
		public string PostalCode { get; set; }
	}
}
