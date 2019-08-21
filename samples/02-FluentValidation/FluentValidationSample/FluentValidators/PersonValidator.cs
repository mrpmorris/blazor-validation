using FluentValidation;
using FluentValidationSample.Models;

namespace FluentValidationSample.FluentValidators
{
	public class PersonValidator : AbstractValidator<Person>
	{
		public PersonValidator()
		{
			RuleFor(x => x.Salutation).NotEmpty();
			RuleFor(x => x.GivenName).NotEmpty();
			RuleFor(x => x.FamilyName).NotEmpty();
			RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress();
			RuleFor(x => x.HomeAddress).SetValidator(new AddressValidator());
		}
	}
}
