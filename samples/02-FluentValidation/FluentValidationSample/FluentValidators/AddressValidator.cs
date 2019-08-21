using FluentValidation;
using FluentValidationSample.Models;

namespace FluentValidationSample.FluentValidators
{
	public class AddressValidator : AbstractValidator<Address>
	{
		public AddressValidator()
		{
			RuleFor(x => x.Line1).NotEmpty();
			RuleFor(x => x.Area).NotEmpty();
			RuleFor(x => x.Town).NotEmpty();
			RuleFor(x => x.Country).NotEmpty();
			RuleFor(x => x.PostalCode).NotEmpty();
		}
	}
}
