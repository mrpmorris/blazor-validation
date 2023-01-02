using FluentValidation;
using FluentValidationSample.Models;

namespace FluentValidationSample.FluentValidators
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
