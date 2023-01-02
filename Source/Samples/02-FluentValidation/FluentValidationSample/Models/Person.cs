using System.Collections.Generic;

namespace FluentValidationSample.Models
{
	public class Person
	{
		public string Salutation { get; set; }
		public string GivenName { get; set; }
		public string MiddleNames { get; set; }
		public string FamilyName { get; set; }
		public string EmailAddress { get; set; }
		public List<NamedAddress> Addresses { get; set; }

		public Person()
		{
			Addresses = new List<NamedAddress>
			{
				new NamedAddress
				{
					Name = "Duplicated address - Home"
				},
				new NamedAddress
				{
					Name = "Duplicated address - Home"
				}
			};
		}
	}
}
