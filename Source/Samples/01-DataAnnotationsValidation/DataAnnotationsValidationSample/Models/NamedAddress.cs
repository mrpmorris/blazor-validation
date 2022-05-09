using System.ComponentModel.DataAnnotations;

namespace DataAnnotationsValidationSample.Models
{
	public class NamedAddress
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public Address Address { get; set; } = new Address();
	}
}
