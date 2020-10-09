using FluentValidation;
using FluentValidationNet5.Models;

namespace FluentValidationNet5.FluentValidators
{
	public class NamedAddressValidator : AbstractValidator<NamedAddress>
	{
		public NamedAddressValidator()
		{
			RuleFor(x => x.Name).NotEmpty();
			RuleFor(x => x.Address).NotNull();
		}
	}
}
