namespace FluentValidationSample.Models
{
	public class Person
	{
		public string Salutation { get; set; }
		public string GivenName { get; set; }
		public string MiddleNames { get; set; }
		public string FamilyName { get; set; }
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
