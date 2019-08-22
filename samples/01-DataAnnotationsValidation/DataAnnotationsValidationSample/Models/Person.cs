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
		public Address HomeAddress { get; set; }
		public Address WorkAddress { get; set; }

		public Person()
		{
			HomeAddress = new Address();
			WorkAddress = new Address();
		}
	}
}
