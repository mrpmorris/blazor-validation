using System.ComponentModel.DataAnnotations;

namespace FluentValidationSample.Models
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
	}
}
