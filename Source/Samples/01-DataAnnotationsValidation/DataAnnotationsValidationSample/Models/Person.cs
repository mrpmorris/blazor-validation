using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAnnotationsValidationSample.Models
{
	public class Person
	{
		[Required]
		public string Salutation { get; set; }
		[Required]
		public string GivenName { get; set; }
		public string MiddleNames { get; set; }
		[Required]
		public string FamilyName { get; set; }
		[Required, EmailAddress]
		public string EmailAddress { get; set; }
		[Required]
		public List<NamedAddress> Addresses { get; set; }

		public Person()
		{
			Addresses = new List<NamedAddress>
			{
				new NamedAddress
				{
					Name = "Home"
				},
				new NamedAddress
				{
					Name = "Work"
				}
			};
		}
	}
}
