﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentValidationSample.Models
{
	public class Address
	{
		public string Line1 { get; set; }
		public string Line2 { get; set; }
		public string Line3 { get; set; }
		public string Town { get; set; }
		public string Area { get; set; }
		public string Country { get; set; }
		public string PostalCode { get; set; }
	}
}
