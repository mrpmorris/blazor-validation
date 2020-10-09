using FluentValidation;
using FluentValidationNet5.Models;

namespace FluentValidationNet5.FluentValidators
{
	public class AddressValidator : AbstractValidator<Address>
	{
		public AddressValidator()
		{
			RuleFor(x => x.Line1).NotEmpty();
			RuleFor(x => x.Town).NotEmpty();
			RuleFor(x => x.Area).NotEmpty();
			RuleFor(x => x.Country).NotEmpty();
			RuleFor(x => x.PostalCode).NotEmpty();
		}
	}
}
